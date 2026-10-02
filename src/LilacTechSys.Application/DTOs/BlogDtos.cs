using System;
using System.Collections.Generic;

namespace LilacTechSys.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectCount { get; set; }
        public int BlogCount { get; set; }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorRole { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public int ReadTimeMinutes { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class BlogPostDetailDto : BlogPostDto
    {
        public string ContentHtml { get; set; } = string.Empty;
        public List<BlogPostDto> RelatedPosts { get; set; } = new List<BlogPostDto>();
    }

    public class CreateBlogRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string ContentHtml { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorRole { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public Guid CategoryId { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public int ReadTimeMinutes { get; set; } = 5;
        public bool IsPublished { get; set; } = true;
    }

    public class UpdateBlogRequest : CreateBlogRequest
    {
    }
}
