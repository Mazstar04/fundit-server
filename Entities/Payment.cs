using fundit_server.Enums;

namespace fundit_server.Entities
{
    public class Payment: BaseEntity
    {
        public decimal Amount { get; set; }
        public string Username { get; set; }
        public string Reference { get; set; }
        public Campaign Campaign { get; set; }
        public Guid CampaignId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}