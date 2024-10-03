using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using fundit_server.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace fundit_server.Implementations.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateAuthToken(IList<Claim> claims, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"]));

            var Sectoken = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }

        public string GenerateToken(IList<Claim> claims)
        {
            

            var refreshToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), 
                signingCredentials: null
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(refreshToken);
        }


    }
}