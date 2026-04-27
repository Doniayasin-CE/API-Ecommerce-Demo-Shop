using System;
using System.Collections.Generic;
using System.Text;

namespace DemoShop.DAL.Models
{
    public enum EntityStatus
    {
        Active = 1,
        InActive = 2
    }
    public class AuditableEntity
    {
        public string CreatedById { get; set; } = null!;
        public string? UpdatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public ApplicationUser CreatedBy { get; set; } = null!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
