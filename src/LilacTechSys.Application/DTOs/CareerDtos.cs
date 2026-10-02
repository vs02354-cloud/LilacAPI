using System;
using System.Collections.Generic;
using LilacTechSys.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LilacTechSys.Application.DTOs
{
    public class JobOpeningDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new List<string>();
        public List<string> Responsibilities { get; set; } = new List<string>();
        public List<string> Benefits { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateJobOpeningRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new List<string>();
        public List<string> Responsibilities { get; set; } = new List<string>();
        public List<string> Benefits { get; set; } = new List<string>();
        public bool IsActive { get; set; } = true;
        public DateTime? Deadline { get; set; }
    }

    public class SubmitApplicationRequest
    {
        public string ApplicantName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? CoverLetter { get; set; }
        public string? PortfolioUrl { get; set; }
        public IFormFile? ResumeFile { get; set; }
    }

    public class JobApplicationDto
    {
        public Guid Id { get; set; }
        public Guid JobOpeningId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ResumeFileName { get; set; } = string.Empty;
        public string ResumeFilePath { get; set; } = string.Empty;
        public string? CoverLetter { get; set; }
        public string? PortfolioUrl { get; set; }
        public ApplicationStatus Status { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateApplicationStatusRequest
    {
        public ApplicationStatus Status { get; set; }
        public string? AdminNotes { get; set; }
    }
}
