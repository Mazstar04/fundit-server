using System.ComponentModel.DataAnnotations;
using fundit_server.Enums;

namespace fundit_server.DTOs
{
    public class InitializePaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Provide a valid Email")]
        public string Email { get; set; }
    }

    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public string CampaignTitle { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public Guid CampaignId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}

