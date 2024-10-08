using System.ComponentModel.DataAnnotations;

namespace fundit_server.DTOs
{
    public class CreateCampaignRequest
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string[] ImagePaths { get; set; }
        [Required]
        [Url]
        public string CoverImagePath { get; set; }

    }

    public class GetCampaignResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountRaised { get; set; }
         public string CoverImagePath { get; set; }
        public string[] ImagePaths { get; set; }
        public DateTime Created { get; set; }
        public string FullName { get; set; }
        public int TotalPayment { get; set; }
    }
    public class GetCampaignDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountRaised { get; set; }
        public string FullName { get; set; }
        public DateTime Created { get; set; }
        public string CoverImagePath { get; set; }
        public string[] ImagePaths { get; set; }
        public int TotalPayment { get; set; }
    }
}