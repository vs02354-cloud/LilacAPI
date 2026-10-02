using System.Collections.Generic;

namespace LilacTechSys.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalServices { get; set; }
        public int TotalProjects { get; set; }
        public int TotalBlogPosts { get; set; }
        public int TotalActiveJobs { get; set; }
        public int PendingContactMessages { get; set; }
        public int PendingQuotes { get; set; }
        public int PendingApplications { get; set; }
        public int TotalSubscribers { get; set; }

        public List<ContactMessageDto> RecentInquiries { get; set; } = new List<ContactMessageDto>();
        public List<QuoteRequestDto> RecentQuotes { get; set; } = new List<QuoteRequestDto>();
        public List<JobApplicationDto> RecentApplications { get; set; } = new List<JobApplicationDto>();
    }
}
