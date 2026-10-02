using System.Linq;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IApplicationDbContext _context;

        public DashboardService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            var totalServices = await _context.Services.CountAsync(s => s.IsActive);
            var totalProjects = await _context.Projects.CountAsync();
            var totalBlogPosts = await _context.BlogPosts.CountAsync(b => b.IsPublished);
            var totalActiveJobs = await _context.JobOpenings.CountAsync(j => j.IsActive);
            var pendingContactMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            var pendingQuotes = await _context.QuoteRequests.CountAsync(q => q.Status == QuoteStatus.New);
            var pendingApplications = await _context.JobApplications.CountAsync(a => a.Status == ApplicationStatus.Pending);
            var totalSubscribers = await _context.Subscribers.CountAsync(s => s.IsActive);

            var recentInquiries = await _context.ContactMessages
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .Select(m => new ContactMessageDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    Phone = m.Phone,
                    Subject = m.Subject,
                    Message = m.Message,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();

            var recentQuotes = await _context.QuoteRequests
                .AsNoTracking()
                .OrderByDescending(q => q.CreatedAt)
                .Take(5)
                .Select(q => new QuoteRequestDto
                {
                    Id = q.Id,
                    FullName = q.FullName,
                    Email = q.Email,
                    Company = q.Company,
                    ServiceRequired = q.ServiceRequired,
                    BudgetRange = q.BudgetRange,
                    Timeline = q.Timeline,
                    Status = q.Status,
                    EstimatedQuoteAmount = q.EstimatedQuoteAmount,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();

            var recentApplications = await _context.JobApplications
                .Include(a => a.JobOpening)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
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
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            var stats = new DashboardStatsDto
            {
                TotalServices = totalServices,
                TotalProjects = totalProjects,
                TotalBlogPosts = totalBlogPosts,
                TotalActiveJobs = totalActiveJobs,
                PendingContactMessages = pendingContactMessages,
                PendingQuotes = pendingQuotes,
                PendingApplications = pendingApplications,
                TotalSubscribers = totalSubscribers,
                RecentInquiries = recentInquiries,
                RecentQuotes = recentQuotes,
                RecentApplications = recentApplications
            };

            return ApiResponse<DashboardStatsDto>.Ok(stats);
        }
    }
}
