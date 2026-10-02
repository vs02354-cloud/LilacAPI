using System;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class Subscriber : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }
}
