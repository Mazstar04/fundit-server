using System.ComponentModel.DataAnnotations;

namespace fundit_server.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Provide a valid Email")]
        public string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Provide a valid PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
         ErrorMessage = "Password must be at least six characters and include a number, a lowercase letter, an uppercase letter, and a special character.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }        
        [Url]
        public string? ProfileImagePath { get; set; }

    }

    public class GetUserStatsResponse{
        public decimal WalletBalance { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public decimal TotalAmountWithdrawn { get; set; }
        public int TotalActiveCampaigns { get; set; }
        public int TotalInactiveCampaigns { get; set; }
    }
}