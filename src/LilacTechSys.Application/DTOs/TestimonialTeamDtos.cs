using System;

namespace LilacTechSys.Application.DTOs
{
    public class TestimonialDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateTestimonialRequest
    {
        public string ClientName { get; set; } = string.Empty;
        public string ClientTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public string Content { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = true;
        public int DisplayOrder { get; set; }
    }

    public class TeamMemberDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? GithubUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTeamMemberRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? GithubUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
