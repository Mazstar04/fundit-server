using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace fundit_server.DTOs
{
    public class UploadImageRequest
    {
        [Required]
        // [FileExtensions(Extensions = "png,jpg,jpeg")]
        public IFormFile file { get; set; }
    }
}