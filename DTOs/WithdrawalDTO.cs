using System.ComponentModel.DataAnnotations;

namespace fundit_server.DTOs
{
   public class WithdrawMoneyRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string AccountName { get; set; }
    }
     public class GetWithdrawalResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public DateTime Created { get; set; }
    }
}