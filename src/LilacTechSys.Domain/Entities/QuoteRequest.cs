using LilacTechSys.Domain.Common;
using LilacTechSys.Domain.Enums;

namespace LilacTechSys.Domain.Entities
{
    public class QuoteRequest : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string ServiceRequired { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty; // e.g. "$5k - $10k", "$10k - $25k", "$25k - $50k", "$50k+"
        public string Timeline { get; set; } = string.Empty; // e.g. "Within 1 Month", "1-3 Months", "Flexible"
        public string ProjectDescription { get; set; } = string.Empty;
        public QuoteStatus Status { get; set; } = QuoteStatus.New;
        public string? AdminNotes { get; set; }
        public decimal? EstimatedQuoteAmount { get; set; }
    }
}
