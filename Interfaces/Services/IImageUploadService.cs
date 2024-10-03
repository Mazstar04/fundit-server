using fundit_server.DTOs;
using fundit_server.Results;
using Microsoft.AspNetCore.Http;

namespace fundit_server.Interfaces.Services
{
    public interface IImageUploadService
    {
        Task<BaseResponse<string>> UploadImageAsync(UploadImageRequest request);
    }
}