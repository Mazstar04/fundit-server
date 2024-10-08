namespace fundit_server.Entities
{
    public class User: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string? HashSalt { get; set; }
        public string? ProfileImagePath { get; set; }
        public decimal WalletBalance { get; set; }
         public List<Campaign> Campaigns { get; set; }
         public List<Payment> Payments { get; set; }
         public List<Withdrawal> Withdrawals { get; set; }
    }
}