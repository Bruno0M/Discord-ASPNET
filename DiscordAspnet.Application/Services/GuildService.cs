using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using System.Net;

namespace DiscordAspnet.Application.Services
{
    public class GuildService : IGuildService
    {

        private readonly IGuildRepository _guildRepository;

        public GuildService(IGuildRepository guildRepository)
        {
            _guildRepository = guildRepository;
        }


        public async Task<ServiceResponse<GuildResponse>> CreateGuildsAsync(GuildRequest guildRequest)
        {
            ServiceResponse<GuildResponse> response = new();

            var guild = new Guild()
            {
                Name = guildRequest.Name,
                OwnerId = guildRequest.OwnerId,
                CreatedAt = DateTime.UtcNow,
            };

            await _guildRepository.CreateGuildsAsync(guild);

            var guildResponse = new GuildResponse(
                guild.Id,
                guild.Name,
                guild.OwnerId,
                guild.CreatedAt);

            response.Data = guildResponse;
            response.Message = "Server created successfully";
            response.Status = HttpStatusCode.OK;

            return response;
        }

        public async Task<ServiceResponse<GuildResponse>> DeleteGuildAsync(Guid guildId, Guid ownerId)
        {
            ServiceResponse<GuildResponse> response = new();

            var guild = await _guildRepository.DeleteGuildAsync(guildId, ownerId);

            if (guild == true)
            {
                response.Message = "Guild succesfully deleted";
                response.Status = HttpStatusCode.OK;
                return response;
            }

            response.Message = "Guild not found";
            response.Status = HttpStatusCode.NotFound;

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GuildResponse>>> GetGuildsAsync()
        {
            ServiceResponse<IEnumerable<GuildResponse>> response = new();

            var guild = await _guildRepository.GetGuildsAsync();

            var guildResponse = guild.Select(g => new GuildResponse(
                g.Id,
                g.Name,
                g.OwnerId,
                g.CreatedAt));

            response.Data = guildResponse;
            response.Status = HttpStatusCode.OK;

            return response;

        }

        public async Task<ServiceResponse<IEnumerable<ChannelResponse>>> GetChannelsGuildAsync(Guid guildId)
        {
            ServiceResponse<IEnumerable<ChannelResponse>> response = new();

            var channel = await _guildRepository.GetChannelsGuildAsync(guildId);

            var channelResponse = channel.Select(c => new ChannelResponse(
                c.Id,
                c.GuildId,
                c.Name,
                c.MessageCount));

            response.Data = channelResponse;
            response.Status = HttpStatusCode.OK;

            return response;

        }
    }
}
