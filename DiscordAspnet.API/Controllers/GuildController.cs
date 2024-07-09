using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscordAspnet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuildsAsync(GuildRequest guildRequest)
        {
            var response = await _guildService.CreateGuildsAsync(guildRequest);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetGuildsAsync()
        {
            var response = await _guildService.GetGuildsAsync();
            return Ok(response);
        }
    }
}
