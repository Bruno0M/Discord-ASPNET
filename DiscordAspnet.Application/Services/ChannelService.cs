using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Domain.Adapters;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using System.Net;
using System.Net.WebSockets;

namespace DiscordAspnet.Application.Services
{
    public class ChannelService : IChannelService
    {

        private readonly IChannelRepository _channelRepository;
        private readonly IWebSocketAdapter _webSocketAdapter;

        public ChannelService(IChannelRepository channelRepository, IWebSocketAdapter webSocketAdapter)
        {
            _channelRepository = channelRepository;
            _webSocketAdapter = webSocketAdapter;
        }

        public async Task<ServiceResponse<ChannelResponse>> CreateChannelAsync(ChannelRequest channelRequest, Guid guildId)
        {
            ServiceResponse<ChannelResponse> response = new();

            var channel = new Channel()
            {
                Name = channelRequest.Name,
                GuildId = guildId,
                MessageCount = 0,
            };

            await _channelRepository.CreateChannelAsync(channel);

            var channelResponse = new ChannelResponse(
                channel.Id,
                channel.GuildId,
                channel.Name,
                channel.MessageCount);

            response.Data = channelResponse;
            response.Message = "Channel succesfully created";
            response.Status = HttpStatusCode.OK;

            return response;
        }

        public async Task<ServiceResponse<ChannelResponse>> DeleteChannelAsync(Guid guildId, Guid channelId, Guid ownerId)
        {
            ServiceResponse<ChannelResponse> response = new();

            var channel = await _channelRepository.DeleteChannelAsync(guildId, channelId, ownerId);

            if (channel == true)
            {
                response.Message = "Channel succesfully deleted";
                response.Status = HttpStatusCode.OK;
                return response;
            }

            response.Message = "Channel not found";
            response.Status = HttpStatusCode.NotFound;

            return response;
        }

        public async Task OnConnectionReceived(WebSocket userConnection, Guid guildId, Guid channelId)
        {
            var channel = new Channel()
            {
                Id = guildId,
                GuildId = channelId
            };

            await _webSocketAdapter.HandleUser(userConnection, channel);
        }
    }
}
