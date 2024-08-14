using DiscordAspnet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscordAspnet.API.Controllers
{
    [Route("Guilds")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _channelService;
        private readonly IWebSocketService _websocketService;

        public ChannelsController(IChannelService channelService, IWebSocketService websocketService)
        {
            _channelService = channelService;
            _websocketService = websocketService;
        }

        [HttpPost("{guildId}/Channels")]
        public async Task<IActionResult> CreateChannelAsync(string name, Guid guildId)
        {
            var channel = await _channelService.CreateChannelAsync(name, guildId);
            return Ok(channel);
        }

        [HttpDelete("{guildId}/{channelId}")]
        public async Task<IActionResult> DeleteChannelAsync(Guid guildId, Guid channelId, Guid ownerId)
        {

            var response = await _channelService.DeleteChannelAsync(guildId, channelId, ownerId);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{guildId}/Channels/{channelId}")]
        public async Task<IActionResult> ConnectChannelGuild(Guid guildId, Guid channelId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync("client"); //client

                await _websocketService.OnConnectionReceived(webSocket, guildId, channelId, User);
                return Ok();
            }

            return BadRequest("WebSocket is not supported.");

        }
    }
}