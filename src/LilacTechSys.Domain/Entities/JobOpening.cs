using System;
using System.Collections.Generic;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class JobOpening : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty; // Remote, Hybrid, Bangalore, IN
        public string Type { get; set; } = string.Empty; // Full-time, Contract
        public string ExperienceLevel { get; set; } = string.Empty; // Junior, Mid-Level, Senior, Lead
        public string Description { get; set; } = string.Empty;
        public string RequirementsJson { get; set; } = "[]";
        public string ResponsibilitiesJson { get; set; } = "[]";
        public string BenefitsJson { get; set; } = "[]";
        public bool IsActive { get; set; } = true;
        public DateTime? Deadline { get; set; }

        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}
