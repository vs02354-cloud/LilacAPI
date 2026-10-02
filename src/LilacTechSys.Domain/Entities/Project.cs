using System;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public string ResultsJson { get; set; } = "[]"; // Metrics e.g. [{"metric": "+140%", "label": "Conversion"}]
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;
        public string GalleryJson { get; set; } = "[]"; // List of image URLs
        public string TechStackJson { get; set; } = "[]"; // e.g. ["React", ".NET", "PostgreSQL", "AWS"]
        public string? ProjectUrl { get; set; }
        
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool IsFeatured { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
    }
}
