using DiscordAspnet.Application.DTOs.UserDTOs;
using DiscordAspnet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DiscordAspnet.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _authService.Login(loginRequest);

            return response.Status == HttpStatusCode.OK
                ? Ok(response)
                : BadRequest(response);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _authService.Register(userRequest);

            return response.Status == HttpStatusCode.OK
                ? Ok(response)
                : BadRequest(response);

        }

    }
}
