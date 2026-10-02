using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class Testimonial : BaseEntity
    {
        public string ClientName { get; set; } = string.Empty;
        public string ClientTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public string Content { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}
