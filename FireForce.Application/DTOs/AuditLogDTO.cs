using System.ComponentModel.DataAnnotations;

namespace FireForce.Application.DTOs
{
    public class AuditLogDTO
    {   
        public int Id { get; set;}
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}