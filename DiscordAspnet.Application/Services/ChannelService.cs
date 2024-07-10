using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using System.Net;

namespace DiscordAspnet.Application.Services
{
    public class ChannelService : IChannelService
    {

        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
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
    }
}
