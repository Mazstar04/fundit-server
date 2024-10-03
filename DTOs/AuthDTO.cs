using System.ComponentModel.DataAnnotations;

namespace fundit_server.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Provide a valid Email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }

    }


}