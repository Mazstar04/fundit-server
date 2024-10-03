
using fundit_server.DTOs;
using fundit_server.Results;

namespace fundit_server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<BaseResponse> RegisterUser(CreateUserRequest request);
        Task<BaseResponse<LoginResponse>> Login(LoginRequest request);
     
       
    }
}