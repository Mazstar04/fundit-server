using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using fundit_server.DTOs;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.Results;

namespace fundit_server.Implementations.Services
{
    public class AuthService : IAuthService
    {

        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public AuthService(IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _config = config;

        }


        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userRepo.GetAsync<User>(u => u.Email == request.Email);
            if (user == null)
            {
                return new BaseResponse<LoginResponse>
                {
                    Succeeded = false,
                    Message = "Invalid Email or password",
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            // Verify password
            byte[] saltBytes = Convert.FromBase64String(user.HashSalt);
            var hashedPassword = HashPassword(request.Password, saltBytes);
            if (hashedPassword != user.PasswordHash)
            {
                return new BaseResponse<LoginResponse>
                {
                    Succeeded = false,
                    Message = "Invalid Email or password",
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Generate tokens
            var accessToken = _tokenService.GenerateAuthToken(claims, _config);
            var refreshToken = _tokenService.GenerateToken(claims);

            return new BaseResponse<LoginResponse>
            {
                Succeeded = true,
                Data = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    Name = user.FirstName + " " + user.LastName,
                    ProfileImagePath = user.ProfileImagePath,
                },
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<BaseResponse> RegisterUser(CreateUserRequest request)
        {
            var existingUser = await _userRepo.GetAsync<User>(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new BaseResponse
                {
                    Succeeded = false,
                    Message = "User with email already exists",
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            // Generate salt and hash password
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            // Create user object
            var newUser = _mapper.Map<User>(request);
            newUser.PasswordHash = hashedPassword;
            newUser.HashSalt = Convert.ToBase64String(salt);

            await _userRepo.AddAsync(newUser);
            await _userRepo.SaveChangesAsync();

            return new BaseResponse
            {
                Succeeded = true,
                Message = "User registered successfully",
                StatusCode = HttpStatusCode.OK
            };
        }


        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        private string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }


    }
}