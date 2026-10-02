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
    public class BlogService : IBlogService
    {
        private readonly IApplicationDbContext _context;

        public BlogService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PagedResult<BlogPostDto>>> GetBlogPostsAsync(QueryParameters queryParams)
        {
            var query = _context.BlogPosts.Include(b => b.Category).AsNoTracking().AsQueryable();

            query = query.Where(b => b.IsPublished);

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(term) ||
                                         b.Excerpt.ToLower().Contains(term) ||
                                         b.AuthorName.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.Category))
            {
                query = query.Where(b => b.Category != null && b.Category.Slug.ToLower() == queryParams.Category.ToLower());
            }

            var totalCount = await query.CountAsync();

            query = queryParams.SortBy?.ToLower() switch
            {
                "views" => queryParams.IsDescending ? query.OrderByDescending(b => b.ViewCount) : query.OrderBy(b => b.ViewCount),
                "title" => queryParams.IsDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                _ => query.OrderByDescending(b => b.PublishedAt)
            };

            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var dtos = items.Select(MapToSummaryDto).ToList();
            var result = new PagedResult<BlogPostDto>(dtos, totalCount, queryParams.PageNumber, queryParams.PageSize);

            return ApiResponse<PagedResult<BlogPostDto>>.Ok(result);
        }

        public async Task<ApiResponse<BlogPostDetailDto>> GetBlogPostBySlugOrIdAsync(string slugOrId)
        {
            BlogPost? post = null;
            if (Guid.TryParse(slugOrId, out var id))
            {
                post = await _context.BlogPosts.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            }
            else
            {
                post = await _context.BlogPosts.Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.Slug.ToLower() == slugOrId.ToLower());
            }

            if (post == null)
                return ApiResponse<BlogPostDetailDto>.Fail("Blog post not found.");

            // Increment view count
            post.ViewCount++;
            await _context.SaveChangesAsync();

            var related = await _context.BlogPosts.Include(b => b.Category)
                .Where(b => b.Id != post.Id && b.IsPublished && b.CategoryId == post.CategoryId)
                .OrderByDescending(b => b.PublishedAt)
                .Take(3)
                .Select(b => MapToSummaryDto(b))
                .ToListAsync();

            var detail = new BlogPostDetailDto
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Excerpt = post.Excerpt,
                CoverImageUrl = post.CoverImageUrl,
                AuthorName = post.AuthorName,
                AuthorRole = post.AuthorRole,
                AuthorAvatarUrl = post.AuthorAvatarUrl,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name ?? "",
                CategorySlug = post.Category?.Slug ?? "",
                Tags = SafeDeserializeList(post.TagsJson),
                ReadTimeMinutes = post.ReadTimeMinutes,
                ViewCount = post.ViewCount,
                IsPublished = post.IsPublished,
                PublishedAt = post.PublishedAt,
                ContentHtml = post.ContentHtml,
                RelatedPosts = related
            };

            return ApiResponse<BlogPostDetailDto>.Ok(detail);
        }

        public async Task<ApiResponse<List<BlogPostDto>>> GetRecentBlogPostsAsync(int count = 3)
        {
            var posts = await _context.BlogPosts.Include(b => b.Category)
                .AsNoTracking()
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.PublishedAt)
                .Take(count)
                .ToListAsync();

            return ApiResponse<List<BlogPostDto>>.Ok(posts.Select(MapToSummaryDto).ToList());
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Projects)
                .Include(c => c.BlogPosts)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    ProjectCount = c.Projects.Count,
                    BlogCount = c.BlogPosts.Count(b => b.IsPublished)
                })
                .ToListAsync();

            return ApiResponse<List<CategoryDto>>.Ok(categories);
        }

        public async Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var exists = await _context.Categories.AnyAsync(c => c.Slug.ToLower() == request.Slug.ToLower());
            if (exists)
                return ApiResponse<CategoryDto>.Fail("A category with this slug already exists.");

            var cat = new Category
            {
                Name = request.Name,
                Slug = request.Slug.ToLower().Trim(),
                Description = request.Description,
                DisplayOrder = request.DisplayOrder
            };

            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();

            return ApiResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = cat.Id,
                Name = cat.Name,
                Slug = cat.Slug,
                Description = cat.Description
            }, "Category created.");
        }

        public async Task<ApiResponse<BlogPostDto>> CreateBlogPostAsync(CreateBlogRequest request)
        {
            var exists = await _context.BlogPosts.AnyAsync(b => b.Slug.ToLower() == request.Slug.ToLower());
            if (exists)
                return ApiResponse<BlogPostDto>.Fail("A post with this slug already exists.");

            var post = new BlogPost
            {
                Title = request.Title,
                Slug = request.Slug.ToLower().Trim(),
                Excerpt = request.Excerpt,
                ContentHtml = request.ContentHtml,
                CoverImageUrl = request.CoverImageUrl,
                AuthorName = request.AuthorName,
                AuthorRole = request.AuthorRole,
                AuthorAvatarUrl = request.AuthorAvatarUrl,
                CategoryId = request.CategoryId,
                TagsJson = JsonSerializer.Serialize(request.Tags ?? new List<string>()),
                ReadTimeMinutes = request.ReadTimeMinutes,
                IsPublished = request.IsPublished,
                PublishedAt = request.IsPublished ? DateTime.UtcNow : null
            };

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            await _context.Entry(post).Reference(b => b.Category).LoadAsync();
            return ApiResponse<BlogPostDto>.Ok(MapToSummaryDto(post), "Blog post published.");
        }

        public async Task<ApiResponse<BlogPostDto>> UpdateBlogPostAsync(Guid id, UpdateBlogRequest request)
        {
            var post = await _context.BlogPosts.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            if (post == null)
                return ApiResponse<BlogPostDto>.Fail("Post not found.");

            post.Title = request.Title;
            post.Slug = request.Slug.ToLower().Trim();
            post.Excerpt = request.Excerpt;
            post.ContentHtml = request.ContentHtml;
            post.CoverImageUrl = request.CoverImageUrl;
            post.AuthorName = request.AuthorName;
            post.AuthorRole = request.AuthorRole;
            post.AuthorAvatarUrl = request.AuthorAvatarUrl;
            post.CategoryId = request.CategoryId;
            post.TagsJson = JsonSerializer.Serialize(request.Tags ?? new List<string>());
            post.ReadTimeMinutes = request.ReadTimeMinutes;
            post.IsPublished = request.IsPublished;
            if (request.IsPublished && post.PublishedAt == null)
                post.PublishedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResponse<BlogPostDto>.Ok(MapToSummaryDto(post), "Blog post updated.");
        }

        public async Task<ApiResponse> DeleteBlogPostAsync(Guid id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
                return ApiResponse.ErrorResult("Post not found.");

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return ApiResponse.SuccessResult("Post deleted.");
        }

        private static BlogPostDto MapToSummaryDto(BlogPost b)
        {
            return new BlogPostDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Excerpt = b.Excerpt,
                CoverImageUrl = b.CoverImageUrl,
                AuthorName = b.AuthorName,
                AuthorRole = b.AuthorRole,
                AuthorAvatarUrl = b.AuthorAvatarUrl,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name ?? "",
                CategorySlug = b.Category?.Slug ?? "",
                Tags = SafeDeserializeList(b.TagsJson),
                ReadTimeMinutes = b.ReadTimeMinutes,
                ViewCount = b.ViewCount,
                IsPublished = b.IsPublished,
                PublishedAt = b.PublishedAt
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
