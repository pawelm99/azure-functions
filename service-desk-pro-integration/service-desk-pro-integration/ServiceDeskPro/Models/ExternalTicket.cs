
namespace ServiceDeskPro_integration.Models
{
    public class ExternalTicket
    {
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? ExternalId { get; set; }
        public string? SlaRuleName { get; set; }
    }
}
