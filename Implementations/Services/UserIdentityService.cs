using System.Security.Claims;
using fundit_server.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace fundit_server.Implementations.Services
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            Guid userId;
            Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);

            return userId;

        }

    }
}