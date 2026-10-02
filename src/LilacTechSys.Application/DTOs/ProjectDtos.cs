using System;
using System.Collections.Generic;

namespace LilacTechSys.Application.DTOs
{
    public class MetricItem
    {
        public string Metric { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public List<string> TechStack { get; set; } = new List<string>();
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProjectDetailDto : ProjectDto
    {
        public string FullDescription { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public List<MetricItem> Results { get; set; } = new List<MetricItem>();
        public string BannerUrl { get; set; } = string.Empty;
        public List<string> Gallery { get; set; } = new List<string>();
        public string? ProjectUrl { get; set; }
    }

    public class CreateProjectRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public List<MetricItem> Results { get; set; } = new List<MetricItem>();
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;
        public List<string> Gallery { get; set; } = new List<string>();
        public List<string> TechStack { get; set; } = new List<string>();
        public string? ProjectUrl { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateProjectRequest : CreateProjectRequest
    {
    }
}
