using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty; // Leadership, Engineering, Design, Cloud & Security
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? GithubUrl { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
