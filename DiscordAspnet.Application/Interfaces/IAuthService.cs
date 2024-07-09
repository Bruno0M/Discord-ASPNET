using DiscordAspnet.Application.DTOs.UserDTOs;
using DiscordAspnet.Application.DTOs;

namespace DiscordAspnet.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponse>> Login(LoginRequest loginRequest);
        Task<ServiceResponse<UserResponse>> Register(UserRequest userRequest);
    }
}
