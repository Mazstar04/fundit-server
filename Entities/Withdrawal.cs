using fundit_server.Enums;

namespace fundit_server.Entities
{
    public class Withdrawal : BaseEntity
    {
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}