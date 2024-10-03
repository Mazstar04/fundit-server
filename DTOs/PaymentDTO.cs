using System.ComponentModel.DataAnnotations;

namespace fundit_server.DTOs
{
    public class MakePaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        
        public Guid? UserId { get; set; }
    }

    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public string CampaignTitle { get; set; }
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public Guid CampaignId { get; set; }
    }
}