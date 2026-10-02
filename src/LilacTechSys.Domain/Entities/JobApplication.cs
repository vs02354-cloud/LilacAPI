using System;
using LilacTechSys.Domain.Common;
using LilacTechSys.Domain.Enums;

namespace LilacTechSys.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public Guid JobOpeningId { get; set; }
        public JobOpening? JobOpening { get; set; }

        public string ApplicantName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ResumeFileName { get; set; } = string.Empty;
        public string ResumeFilePath { get; set; } = string.Empty;
        public string? CoverLetter { get; set; }
        public string? PortfolioUrl { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public string? AdminNotes { get; set; }
    }
}
