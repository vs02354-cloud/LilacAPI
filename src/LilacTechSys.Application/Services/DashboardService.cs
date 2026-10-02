using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IApplicationDbContext _context;

        public DashboardService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            var now = DateTime.UtcNow;
            var todayUtc = now.Date;

            // Global entity counts
            var totalServices = await _context.Services.CountAsync(s => s.IsActive);
            var totalProjects = await _context.Projects.CountAsync();
            var totalBlogPosts = await _context.BlogPosts.CountAsync(b => b.IsPublished);
            var totalActiveJobs = await _context.JobOpenings.CountAsync(j => j.IsActive);
            var totalSubscribers = await _context.Subscribers.CountAsync(s => s.IsActive);

            // Fetch quotes
            var allQuotes = await _context.QuoteRequests
                .AsNoTracking()
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            // Fetch inquiries
            var allInquiries = await _context.ContactMessages
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            // Fetch applications
            var allApplications = await _context.JobApplications
                .Include(a => a.JobOpening)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            // Unified tracked submissions
            var trackedList = new List<TrackedSubmissionItemDto>();

            // 1. Process Quotes
            foreach (var q in allQuotes)
            {
                var isToday = q.CreatedAt.Date == todayUtc;
                string stage;
                if (q.Status == QuoteStatus.New) stage = "Pending";
                else if (q.Status == QuoteStatus.Reviewing || q.Status == QuoteStatus.Contacted) stage = "FollowUp";
                else stage = "Completed";

                string priority = "Normal";
                if (!string.IsNullOrEmpty(q.BudgetRange) && (q.BudgetRange.Contains("50k") || q.BudgetRange.Contains("25k")))
                {
                    priority = "High";
                }
                if (!string.IsNullOrEmpty(q.Timeline) && q.Timeline.ToLower().Contains("month") && !q.Timeline.Contains("3"))
                {
                    priority = "Urgent";
                }

                trackedList.Add(new TrackedSubmissionItemDto
                {
                    Id = q.Id.ToString(),
                    TrackingCode = $"RFQ-{q.Id.ToString()[..8].ToUpper()}",
                    Type = "Quote",
                    ClientName = q.FullName,
                    Email = q.Email,
                    Phone = q.Phone ?? "",
                    Organization = q.Company ?? "Individual Client",
                    TitleOrService = q.ServiceRequired ?? "Full-Stack Development",
                    Stage = stage,
                    SpecificStatus = q.Status.ToString(),
                    StatusCode = (int)q.Status,
                    Priority = priority,
                    BudgetOrScope = q.BudgetRange ?? "$10k - $25k",
                    CreatedAt = q.CreatedAt,
                    IsToday = isToday
                });
            }

            // 2. Process Inquiries
            foreach (var m in allInquiries)
            {
                var isToday = m.CreatedAt.Date == todayUtc;
                string stage;
                if (!m.IsRead) stage = "Pending";
                else if (m.RespondedAt == null) stage = "FollowUp";
                else stage = "Completed";

                trackedList.Add(new TrackedSubmissionItemDto
                {
                    Id = m.Id.ToString(),
                    TrackingCode = $"INQ-{m.Id.ToString()[..8].ToUpper()}",
                    Type = "Inquiry",
                    ClientName = m.FullName,
                    Email = m.Email,
                    Phone = m.Phone ?? "",
                    Organization = "Inbound Prospect",
                    TitleOrService = m.Subject ?? "General Inquiry",
                    Stage = stage,
                    SpecificStatus = m.IsRead ? (m.RespondedAt != null ? "Responded" : "In Review") : "Unread",
                    StatusCode = m.IsRead ? 1 : 0,
                    Priority = "Normal",
                    BudgetOrScope = m.Message.Length > 50 ? m.Message[..50] + "..." : m.Message,
                    CreatedAt = m.CreatedAt,
                    IsToday = isToday
                });
            }

            // 3. Process Applications
            foreach (var a in allApplications)
            {
                var isToday = a.CreatedAt.Date == todayUtc;
                string stage;
                if (a.Status == ApplicationStatus.Pending) stage = "Pending";
                else if (a.Status == ApplicationStatus.Reviewed || a.Status == ApplicationStatus.Shortlisted || a.Status == ApplicationStatus.InterviewScheduled) stage = "FollowUp";
                else stage = "Completed";

                trackedList.Add(new TrackedSubmissionItemDto
                {
                    Id = a.Id.ToString(),
                    TrackingCode = $"APP-{a.Id.ToString()[..8].ToUpper()}",
                    Type = "Career",
                    ClientName = a.ApplicantName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Organization = "Candidate",
                    TitleOrService = a.JobOpening != null ? a.JobOpening.Title : "Position Applicant",
                    Stage = stage,
                    SpecificStatus = a.Status.ToString(),
                    StatusCode = (int)a.Status,
                    Priority = "Normal",
                    BudgetOrScope = a.ResumeFileName ?? "Resume.pdf",
                    CreatedAt = a.CreatedAt,
                    IsToday = isToday
                });
            }

            // Order submissions by date descending
            trackedList = trackedList.OrderByDescending(x => x.CreatedAt).ToList();

            // Calculate Day Progress Metrics
            var receivedToday = trackedList.Count(x => x.IsToday);
            var completedToday = trackedList.Count(x => x.IsToday && x.Stage == "Completed");
            var currentlyPending = trackedList.Count(x => x.Stage == "Pending");
            var inFollowUp = trackedList.Count(x => x.Stage == "FollowUp");
            var totalProcessedToday = completedToday + inFollowUp;
            var completionRate = receivedToday > 0 
                ? Math.Round(((double)totalProcessedToday / receivedToday) * 100, 1) 
                : 100.0;

            var todayProgress = new DayProgressMetricDto
            {
                ReceivedToday = receivedToday,
                CompletedToday = completedToday,
                CurrentlyPending = currentlyPending,
                InFollowUpStage = inFollowUp,
                CompletionRate = completionRate
            };

            // Build 7-Day Trend
            var weeklyTrend = new List<DailyTrendItemDto>();
            for (int i = 6; i >= 0; i--)
            {
                var d = todayUtc.AddDays(-i);
                var daySubmissions = trackedList.Where(x => x.CreatedAt.Date == d).ToList();
                weeklyTrend.Add(new DailyTrendItemDto
                {
                    Day = d.ToString("ddd"),
                    Date = d.ToString("MMM dd"),
                    Received = daySubmissions.Count,
                    Completed = daySubmissions.Count(x => x.Stage == "Completed"),
                    FollowUp = daySubmissions.Count(x => x.Stage == "FollowUp")
                });
            }

            // Recents for secondary widgets
            var recentInquiries = allInquiries.Take(5).Select(m => new ContactMessageDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                Subject = m.Subject,
                Message = m.Message,
                IsRead = m.IsRead,
                CreatedAt = m.CreatedAt
            }).ToList();

            var recentQuotes = allQuotes.Take(5).Select(q => new QuoteRequestDto
            {
                Id = q.Id,
                FullName = q.FullName,
                Email = q.Email,
                Company = q.Company,
                ServiceRequired = q.ServiceRequired,
                BudgetRange = q.BudgetRange,
                Timeline = q.Timeline,
                Status = q.Status,
                CreatedAt = q.CreatedAt
            }).ToList();

            var recentApplications = allApplications.Take(5).Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobOpeningId = a.JobOpeningId,
                JobTitle = a.JobOpening != null ? a.JobOpening.Title : "",
                ApplicantName = a.ApplicantName,
                Email = a.Email,
                Phone = a.Phone,
                ResumeFileName = a.ResumeFileName,
                ResumeFilePath = a.ResumeFilePath,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            }).ToList();

            var stats = new DashboardStatsDto
            {
                TodayProgress = todayProgress,
                WeeklyTrend = weeklyTrend,
                Submissions = trackedList,
                TotalServices = totalServices,
                TotalProjects = totalProjects,
                TotalBlogPosts = totalBlogPosts,
                TotalActiveJobs = totalActiveJobs,
                PendingContactMessages = currentlyPending,
                PendingQuotes = trackedList.Count(x => x.Type == "Quote" && x.Stage == "Pending"),
                PendingApplications = trackedList.Count(x => x.Type == "Career" && x.Stage == "Pending"),
                TotalSubscribers = totalSubscribers,
                RecentInquiries = recentInquiries,
                RecentQuotes = recentQuotes,
                RecentApplications = recentApplications
            };

            return ApiResponse<DashboardStatsDto>.Ok(stats);
        }

        public async Task<ApiResponse> UpdateSubmissionStageAsync(string type, Guid id, string newStage)
        {
            var normalizedType = type.ToLower();
            var normalizedStage = newStage.ToLower();

            if (normalizedType == "quote")
            {
                var quote = await _context.QuoteRequests.FindAsync(id);
                if (quote == null) return ApiResponse.ErrorResult("Quote request not found.");

                if (normalizedStage == "pending") quote.Status = QuoteStatus.New;
                else if (normalizedStage == "followup" || normalizedStage == "follow-up") quote.Status = QuoteStatus.Reviewing;
                else if (normalizedStage == "completed") quote.Status = QuoteStatus.Quoted;
                else quote.Status = QuoteStatus.Reviewing;

                await _context.SaveChangesAsync();
                return ApiResponse.SuccessResult($"Quote {quote.FullName} updated to {newStage}.");
            }
            else if (normalizedType == "inquiry")
            {
                var msg = await _context.ContactMessages.FindAsync(id);
                if (msg == null) return ApiResponse.ErrorResult("Contact message not found.");

                if (normalizedStage == "pending") { msg.IsRead = false; msg.RespondedAt = null; }
                else if (normalizedStage == "followup" || normalizedStage == "follow-up") { msg.IsRead = true; msg.RespondedAt = null; }
                else if (normalizedStage == "completed") { msg.IsRead = true; msg.RespondedAt = DateTime.UtcNow; }

                await _context.SaveChangesAsync();
                return ApiResponse.SuccessResult($"Inquiry from {msg.FullName} updated to {newStage}.");
            }
            else if (normalizedType == "career" || normalizedType == "jobapplication")
            {
                var app = await _context.JobApplications.FindAsync(id);
                if (app == null) return ApiResponse.ErrorResult("Job application not found.");

                if (normalizedStage == "pending") app.Status = ApplicationStatus.Pending;
                else if (normalizedStage == "followup" || normalizedStage == "follow-up") app.Status = ApplicationStatus.Shortlisted;
                else if (normalizedStage == "completed") app.Status = ApplicationStatus.Hired;

                await _context.SaveChangesAsync();
                return ApiResponse.SuccessResult($"Application for {app.ApplicantName} updated to {newStage}.");
            }

            return ApiResponse.ErrorResult("Unknown submission type.");
        }
    }
}
