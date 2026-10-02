using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string DetailedDescription { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty; // Lucide icon name or svg identifier
        public string FeaturesJson { get; set; } = "[]"; // List of feature bullet points
        public string BenefitsJson { get; set; } = "[]"; // Key enterprise benefits
        public string TechnologiesJson { get; set; } = "[]"; // Tech stack utilized
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
