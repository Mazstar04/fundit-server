namespace fundit_server.Entities
{
    public class Campaign: BaseEntity
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string[] ImagePaths { get; set; }
        public string CoverImagePath { get; set; }
        public List<Payment> Payments { get; set; }
    }
}