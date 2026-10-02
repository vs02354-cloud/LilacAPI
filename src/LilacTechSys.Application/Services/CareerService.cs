using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Entities;
using LilacTechSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Application.Services
{
    public class CareerService : ICareerService
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly IEmailNotificationService _emailService;

        public CareerService(IApplicationDbContext context, IFileStorageService fileStorage, IEmailNotificationService emailService)
        {
            _context = context;
            _fileStorage = fileStorage;
            _emailService = emailService;
        }

        public async Task<ApiResponse<List<JobOpeningDto>>> GetActiveJobOpeningsAsync()
        {
            var jobs = await _context.JobOpenings
                .AsNoTracking()
                .Where(j => j.IsActive && (j.Deadline == null || j.Deadline >= DateTime.UtcNow))
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return ApiResponse<List<JobOpeningDto>>.Ok(jobs.Select(MapToDto).ToList());
        }

        public async Task<ApiResponse<JobOpeningDto>> GetJobOpeningBySlugOrIdAsync(string slugOrId)
        {
            JobOpening? job = null;
            if (Guid.TryParse(slugOrId, out var id))
            {
                job = await _context.JobOpenings.FindAsync(id);
            }
            else
            {
                job = await _context.JobOpenings.FirstOrDefaultAsync(j => j.Slug.ToLower() == slugOrId.ToLower());
            }

            if (job == null)
                return ApiResponse<JobOpeningDto>.Fail("Job opening not found.");

            return ApiResponse<JobOpeningDto>.Ok(MapToDto(job));
        }

        public async Task<ApiResponse<JobOpeningDto>> CreateJobOpeningAsync(CreateJobOpeningRequest request)
        {
            var exists = await _context.JobOpenings.AnyAsync(j => j.Slug.ToLower() == request.Slug.ToLower());
            if (exists)
                return ApiResponse<JobOpeningDto>.Fail("A job opening with this slug already exists.");

            var job = new JobOpening
            {
                Title = request.Title,
                Slug = request.Slug.ToLower().Trim(),
                Department = request.Department,
                Location = request.Location,
                Type = request.Type,
                ExperienceLevel = request.ExperienceLevel,
                Description = request.Description,
                RequirementsJson = JsonSerializer.Serialize(request.Requirements ?? new List<string>()),
                ResponsibilitiesJson = JsonSerializer.Serialize(request.Responsibilities ?? new List<string>()),
                BenefitsJson = JsonSerializer.Serialize(request.Benefits ?? new List<string>()),
                IsActive = request.IsActive,
                Deadline = request.Deadline
            };

            _context.JobOpenings.Add(job);
            await _context.SaveChangesAsync();

            return ApiResponse<JobOpeningDto>.Ok(MapToDto(job), "Job opening created.");
        }

        public async Task<ApiResponse<JobOpeningDto>> UpdateJobOpeningAsync(Guid id, CreateJobOpeningRequest request)
        {
            var job = await _context.JobOpenings.FindAsync(id);
            if (job == null)
                return ApiResponse<JobOpeningDto>.Fail("Job opening not found.");

            job.Title = request.Title;
            job.Slug = request.Slug.ToLower().Trim();
            job.Department = request.Department;
            job.Location = request.Location;
            job.Type = request.Type;
            job.ExperienceLevel = request.ExperienceLevel;
            job.Description = request.Description;
            job.RequirementsJson = JsonSerializer.Serialize(request.Requirements ?? new List<string>());
            job.ResponsibilitiesJson = JsonSerializer.Serialize(request.Responsibilities ?? new List<string>());
            job.BenefitsJson = JsonSerializer.Serialize(request.Benefits ?? new List<string>());
            job.IsActive = request.IsActive;
            job.Deadline = request.Deadline;
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse<JobOpeningDto>.Ok(MapToDto(job), "Job opening updated.");
        }

        public async Task<ApiResponse> DeleteJobOpeningAsync(Guid id)
        {
            var job = await _context.JobOpenings.FindAsync(id);
            if (job == null)
                return ApiResponse.ErrorResult("Job opening not found.");

            _context.JobOpenings.Remove(job);
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Job opening deleted.");
        }

        public async Task<ApiResponse<JobApplicationDto>> SubmitApplicationAsync(Guid jobOpeningId, SubmitApplicationRequest request)
        {
            var job = await _context.JobOpenings.FindAsync(jobOpeningId);
            if (job == null || !job.IsActive)
                return ApiResponse<JobApplicationDto>.Fail("Job opening is no longer accepting applications.");

            string resumePath = "";
            string originalFileName = "";

            if (request.ResumeFile != null)
            {
                originalFileName = request.ResumeFile.FileName;
                resumePath = await _fileStorage.SaveResumeFileAsync(request.ResumeFile);
            }

            var application = new JobApplication
            {
                JobOpeningId = jobOpeningId,
                ApplicantName = request.ApplicantName,
                Email = request.Email,
                Phone = request.Phone,
                ResumeFileName = originalFileName,
                ResumeFilePath = resumePath,
                CoverLetter = request.CoverLetter,
                PortfolioUrl = request.PortfolioUrl,
                Status = ApplicationStatus.Pending
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            // Dispatch notification
            await _emailService.SendApplicationNotificationAsync(request.ApplicantName, request.Email, job.Title);

            var dto = new JobApplicationDto
            {
                Id = application.Id,
                JobOpeningId = job.Id,
                JobTitle = job.Title,
                ApplicantName = application.ApplicantName,
                Email = application.Email,
                Phone = application.Phone,
                ResumeFileName = application.ResumeFileName,
                ResumeFilePath = application.ResumeFilePath,
                CoverLetter = application.CoverLetter,
                PortfolioUrl = application.PortfolioUrl,
                Status = application.Status,
                CreatedAt = application.CreatedAt
            };

            return ApiResponse<JobApplicationDto>.Ok(dto, "Application submitted successfully.");
        }

        public async Task<ApiResponse<PagedResult<JobApplicationDto>>> GetApplicationsAsync(QueryParameters queryParams)
        {
            var query = _context.JobApplications.Include(a => a.JobOpening).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(a => a.ApplicantName.ToLower().Contains(term) ||
                                         a.Email.ToLower().Contains(term) ||
                                         (a.JobOpening != null && a.JobOpening.Title.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(a => a.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    JobOpeningId = a.JobOpeningId,
                    JobTitle = a.JobOpening != null ? a.JobOpening.Title : "",
                    ApplicantName = a.ApplicantName,
                    Email = a.Email,
                    Phone = a.Phone,
                    ResumeFileName = a.ResumeFileName,
                    ResumeFilePath = a.ResumeFilePath,
                    CoverLetter = a.CoverLetter,
                    PortfolioUrl = a.PortfolioUrl,
                    Status = a.Status,
                    AdminNotes = a.AdminNotes,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            var result = new PagedResult<JobApplicationDto>(items, totalCount, queryParams.PageNumber, queryParams.PageSize);
            return ApiResponse<PagedResult<JobApplicationDto>>.Ok(result);
        }

        public async Task<ApiResponse> UpdateApplicationStatusAsync(Guid applicationId, UpdateApplicationStatusRequest request)
        {
            var app = await _context.JobApplications.FindAsync(applicationId);
            if (app == null)
                return ApiResponse.ErrorResult("Application not found.");

            app.Status = request.Status;
            app.AdminNotes = request.AdminNotes;
            app.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Status updated.");
        }

        private static JobOpeningDto MapToDto(JobOpening j)
        {
            return new JobOpeningDto
            {
                Id = j.Id,
                Title = j.Title,
                Slug = j.Slug,
                Department = j.Department,
                Location = j.Location,
                Type = j.Type,
                ExperienceLevel = j.ExperienceLevel,
                Description = j.Description,
                Requirements = SafeDeserializeList(j.RequirementsJson),
                Responsibilities = SafeDeserializeList(j.ResponsibilitiesJson),
                Benefits = SafeDeserializeList(j.BenefitsJson),
                IsActive = j.IsActive,
                Deadline = j.Deadline,
                CreatedAt = j.CreatedAt
            };
        }

        private static List<string> SafeDeserializeList(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
            catch { return new List<string>(); }
        }
    }
}
