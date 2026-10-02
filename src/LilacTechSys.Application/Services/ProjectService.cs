using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagedResult<ProjectDto>>> GetProjectsAsync(QueryParameters queryParams)
        {
            var query = _context.Projects.Include(p => p.Category).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(term) ||
                                         p.Summary.ToLower().Contains(term) ||
                                         p.ClientName.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.Category))
            {
                query = query.Where(p => p.Category != null && p.Category.Slug.ToLower() == queryParams.Category.ToLower());
            }

            var totalCount = await query.CountAsync();

            query = queryParams.SortBy?.ToLower() switch
            {
                "title" => queryParams.IsDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "createdat" => queryParams.IsDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => query.OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreatedAt)
            };

            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var dtos = items.Select(MapToSummaryDto).ToList();
            var result = new PagedResult<ProjectDto>(dtos, totalCount, queryParams.PageNumber, queryParams.PageSize);

            return ApiResponse<PagedResult<ProjectDto>>.Ok(result);
        }

        public async Task<ApiResponse<ProjectDetailDto>> GetProjectBySlugOrIdAsync(string slugOrId)
        {
            Project? project = null;
            if (Guid.TryParse(slugOrId, out var id))
            {
                project = await _context.Projects.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            }
            else
            {
                project = await _context.Projects.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Slug.ToLower() == slugOrId.ToLower());
            }

            if (project == null)
                return ApiResponse<ProjectDetailDto>.Fail("Project not found.");

            return ApiResponse<ProjectDetailDto>.Ok(MapToDetailDto(project));
        }

        public async Task<ApiResponse<List<ProjectDto>>> GetFeaturedProjectsAsync()
        {
            var projects = await _context.Projects.Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.IsFeatured)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return ApiResponse<List<ProjectDto>>.Ok(projects.Select(MapToSummaryDto).ToList());
        }

        public async Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectRequest request)
        {
            var existing = await _context.Projects.AnyAsync(p => p.Slug.ToLower() == request.Slug.ToLower());
            if (existing)
                return ApiResponse<ProjectDto>.Fail("A project with this URL slug already exists.");

            var project = new Project
            {
                Title = request.Title,
                Slug = request.Slug.ToLower().Trim(),
                ClientName = request.ClientName,
                Summary = request.Summary,
                FullDescription = request.FullDescription,
                Challenge = request.Challenge,
                Solution = request.Solution,
                ResultsJson = JsonSerializer.Serialize(request.Results ?? new List<MetricItem>()),
                ThumbnailUrl = request.ThumbnailUrl,
                BannerUrl = request.BannerUrl,
                GalleryJson = JsonSerializer.Serialize(request.Gallery ?? new List<string>()),
                TechStackJson = JsonSerializer.Serialize(request.TechStack ?? new List<string>()),
                ProjectUrl = request.ProjectUrl,
                CategoryId = request.CategoryId,
                IsFeatured = request.IsFeatured,
                DisplayOrder = request.DisplayOrder
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            await _context.Entry(project).Reference(p => p.Category).LoadAsync();
            return ApiResponse<ProjectDto>.Ok(MapToSummaryDto(project), "Project created successfully.");
        }

        public async Task<ApiResponse<ProjectDto>> UpdateProjectAsync(Guid id, UpdateProjectRequest request)
        {
            var project = await _context.Projects.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return ApiResponse<ProjectDto>.Fail("Project not found.");

            project.Title = request.Title;
            project.Slug = request.Slug.ToLower().Trim();
            project.ClientName = request.ClientName;
            project.Summary = request.Summary;
            project.FullDescription = request.FullDescription;
            project.Challenge = request.Challenge;
            project.Solution = request.Solution;
            project.ResultsJson = JsonSerializer.Serialize(request.Results ?? new List<MetricItem>());
            project.ThumbnailUrl = request.ThumbnailUrl;
            project.BannerUrl = request.BannerUrl;
            project.GalleryJson = JsonSerializer.Serialize(request.Gallery ?? new List<string>());
            project.TechStackJson = JsonSerializer.Serialize(request.TechStack ?? new List<string>());
            project.ProjectUrl = request.ProjectUrl;
            project.CategoryId = request.CategoryId;
            project.IsFeatured = request.IsFeatured;
            project.DisplayOrder = request.DisplayOrder;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse<ProjectDto>.Ok(MapToSummaryDto(project), "Project updated successfully.");
        }

        public async Task<ApiResponse> DeleteProjectAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return ApiResponse.ErrorResult("Project not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Project deleted successfully.");
        }

        private static ProjectDto MapToSummaryDto(Project p)
        {
            return new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                ClientName = p.ClientName,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl,
                TechStack = SafeDeserializeList(p.TechStackJson),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "",
                CategorySlug = p.Category?.Slug ?? "",
                IsFeatured = p.IsFeatured,
                DisplayOrder = p.DisplayOrder,
                CreatedAt = p.CreatedAt
            };
        }

        private static ProjectDetailDto MapToDetailDto(Project p)
        {
            return new ProjectDetailDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                ClientName = p.ClientName,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl,
                TechStack = SafeDeserializeList(p.TechStackJson),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "",
                CategorySlug = p.Category?.Slug ?? "",
                IsFeatured = p.IsFeatured,
                DisplayOrder = p.DisplayOrder,
                CreatedAt = p.CreatedAt,
                FullDescription = p.FullDescription,
                Challenge = p.Challenge,
                Solution = p.Solution,
                Results = SafeDeserializeResults(p.ResultsJson),
                BannerUrl = p.BannerUrl,
                Gallery = SafeDeserializeList(p.GalleryJson),
                ProjectUrl = p.ProjectUrl
            };
        }

        private static List<string> SafeDeserializeList(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
            catch { return new List<string>(); }
        }

        private static List<MetricItem> SafeDeserializeResults(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<MetricItem>();
            try { return JsonSerializer.Deserialize<List<MetricItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<MetricItem>(); }
            catch { return new List<MetricItem>(); }
        }
    }
}
