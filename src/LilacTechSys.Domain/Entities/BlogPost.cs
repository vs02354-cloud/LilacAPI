using System;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class BlogPost : BaseEntity
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
        public Category? Category { get; set; }

        public string TagsJson { get; set; } = "[]"; // e.g. ["Cloud", "Security", "AI"]
        public int ReadTimeMinutes { get; set; } = 5;
        public int ViewCount { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
        public DateTime? PublishedAt { get; set; } = DateTime.UtcNow;
    }
}
