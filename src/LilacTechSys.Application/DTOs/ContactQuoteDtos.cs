using System;
using LilacTechSys.Domain.Enums;

namespace LilacTechSys.Application.DTOs
{
    public class SubmitContactRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ContactMessageDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string? ResponseNotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SubmitQuoteRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string ServiceRequired { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty;
        public string Timeline { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
    }

    public class QuoteRequestDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string ServiceRequired { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty;
        public string Timeline { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public QuoteStatus Status { get; set; }
        public string? AdminNotes { get; set; }
        public decimal? EstimatedQuoteAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateQuoteStatusRequest
    {
        public QuoteStatus Status { get; set; }
        public string? AdminNotes { get; set; }
        public decimal? EstimatedQuoteAmount { get; set; }
    }

    public class SubscribeRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class SubscriberDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime SubscribedAt { get; set; }
    }
}
