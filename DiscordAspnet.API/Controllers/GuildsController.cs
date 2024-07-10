using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiscordAspnet.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GuildsController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildsController(IGuildService guildService)
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

        [HttpGet("{guildId}")]
        public async Task<IActionResult> GetChannelsGuildAsync(Guid guildId)
        {
            var channel = await _guildService.GetChannelsGuildAsync(guildId);
            return Ok(channel);
        }

        [HttpDelete("{guildId}")]
        public async Task<IActionResult> DeleteChannelAsync(Guid guildId, Guid ownerId)
        {
            var response = await _guildService.DeleteGuildAsync(guildId, ownerId);
            return Ok(response);
        }
    }
}