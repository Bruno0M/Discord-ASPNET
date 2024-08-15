using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.UserDTOs;
using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace DiscordAspnet.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;



        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse<UserResponse>> Register(UserRequest userRequest)
        {
            ServiceResponse<UserResponse> response = new();


            var user = new ApplicationUser()
            {
                Email = userRequest.Email,
                UserName = userRequest.Username,
                CreatedAt = DateTime.UtcNow,
                MessageCount = 0,
                ServersCount = 0
            };

            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                response.Message = string.Join(" ", errors);
                response.Status = HttpStatusCode.BadRequest;
                return response;
            }

            var userResponse = new UserResponse(
                user.Id,
                user.Email,
                user.ServersCount,
                user.MessageCount,
                user.CreatedAt);

            response.Data = userResponse;
            response.Message = "User successfully created";
            response.Status = HttpStatusCode.OK;

            return response;
        }

        public async Task<ServiceResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            ServiceResponse<LoginResponse> response = new();

            if (string.IsNullOrEmpty(loginRequest.Credential) || string.IsNullOrEmpty(loginRequest.Password))
            {
                response.Data = null;
                response.Message = "Invalid Credentials";
                response.Status = HttpStatusCode.BadRequest;
                return response;
            }

            var userExists = await _userManager.FindByEmailAsync(loginRequest.Credential);

            if (userExists == null)
            {
                userExists = await _userManager.FindByNameAsync(loginRequest.Credential);
            }

            if (userExists == null || !await _userManager.CheckPasswordAsync(userExists, loginRequest.Password))
            {
                response.Data = null;
                response.Message = "Invalid Credentials";
                response.Status = HttpStatusCode.Unauthorized;
                return response;
            }

            var token = _tokenService.GenerateToken(userExists);

            var loginResponse = new LoginResponse(
                userExists.Id,
                userExists.UserName,
                userExists.CreatedAt,
                token);

            response.Data = loginResponse;
            response.Message = "Successfully logged in";
            response.Status = HttpStatusCode.OK;

            return response;
        }

    }
}
