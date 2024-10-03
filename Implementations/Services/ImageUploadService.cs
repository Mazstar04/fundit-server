using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using fundit_server.Results;
using Microsoft.AspNetCore.Http;

namespace fundit_server.Implementations.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly Cloudinary _cloudinary;

        public ImageUploadService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ;
        }

        public async Task<BaseResponse<string>> UploadImageAsync(UploadImageRequest request)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.file.FileName, request.file.OpenReadStream()),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new BaseResponse<string>
            {
                Message = uploadResult?.Url != null ? "Image Uploaded successfully" : "Image Upload failed",
                Succeeded = uploadResult?.Url != null,
                StatusCode = uploadResult?.Url != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                Data = uploadResult?.Url?.ToString()
            };
        }
    }
}