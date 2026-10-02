using System;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class ContactMessage : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime? RespondedAt { get; set; }
        public string? ResponseNotes { get; set; }
    }
}
