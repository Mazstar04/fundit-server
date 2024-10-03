namespace fundit_server.Entities
{
    public class Payment: BaseEntity
    {
        public decimal Amount { get; set; }
        public User User { get; set; }
        public Guid? UserId { get; set; }
        public Campaign Campaign { get; set; }
        public Guid CampaignId { get; set; }
    }
}