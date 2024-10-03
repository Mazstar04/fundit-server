using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace fundit_server.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateAuthToken(IList<Claim> claims, IConfiguration _config);
        public string GenerateToken(IList<Claim> claims);
    }
}