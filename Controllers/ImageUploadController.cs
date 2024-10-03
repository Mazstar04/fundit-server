using fundit_server.DTOs;
using fundit_server.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fundit_server.Controllers;

[ApiController]
[Route("api/v1/image")]
public class ImageUploadController : BaseApiController
{

    private readonly IImageUploadService _imageService;

    public ImageUploadController(IImageUploadService imageService)
    {
        _imageService = imageService;
    }
    
    [Route("upload")]
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm]UploadImageRequest request)
    {
        var result = await _imageService.UploadImageAsync(request);
        return Respond(result);

    }

}
