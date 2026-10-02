using System;
using System.Collections.Generic;

namespace LilacTechSys.Application.DTOs
{
    public class DayProgressMetricDto
    {
        public int ReceivedToday { get; set; }
        public int CompletedToday { get; set; }
        public int CurrentlyPending { get; set; }
        public int InFollowUpStage { get; set; }
        public double CompletionRate { get; set; }
    }

    public class DailyTrendItemDto
    {
        public string Day { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int Received { get; set; }
        public int Completed { get; set; }
        public int FollowUp { get; set; }
    }

    public class TrackedSubmissionItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string TrackingCode { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Quote", "Inquiry", "Career"
        public string ClientName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string TitleOrService { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty; // "Pending", "FollowUp", "Completed"
        public string SpecificStatus { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string Priority { get; set; } = string.Empty; // "High", "Medium", "Normal"
        public string BudgetOrScope { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsToday { get; set; }
    }

    public class UpdateSubmissionStageRequest
    {
        public string Stage { get; set; } = string.Empty; // "Pending", "FollowUp", "Completed"
    }

    public class DashboardStatsDto
    {
        public DayProgressMetricDto TodayProgress { get; set; } = new();
        public List<DailyTrendItemDto> WeeklyTrend { get; set; } = new();
        public List<TrackedSubmissionItemDto> Submissions { get; set; } = new();

        public int TotalServices { get; set; }
        public int TotalProjects { get; set; }
        public int TotalBlogPosts { get; set; }
        public int TotalActiveJobs { get; set; }
        public int PendingContactMessages { get; set; }
        public int PendingQuotes { get; set; }
        public int PendingApplications { get; set; }
        public int TotalSubscribers { get; set; }

        public List<ContactMessageDto> RecentInquiries { get; set; } = new();
        public List<QuoteRequestDto> RecentQuotes { get; set; } = new();
        public List<JobApplicationDto> RecentApplications { get; set; } = new();
    }
}
