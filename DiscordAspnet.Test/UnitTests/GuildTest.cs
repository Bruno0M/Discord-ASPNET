using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Application.Services;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using Moq;
using System.Net;

namespace DiscordAspnet.Test.UnitTests
{
    public class GuildTest
    {
        private readonly Mock<IGuildRepository> _guildRepositoryMock;
        private readonly GuildService _guildService;

        public GuildTest()
        {
            _guildRepositoryMock = new Mock<IGuildRepository>();
            _guildService = new GuildService(_guildRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateGuildsAsync_ValidGuildRequest_ReturnsSuccessResponse()
        {
            //Arrange

            var guildRequest = new GuildRequest("GuildTest", Guid.NewGuid());

            //Act
            var response = await _guildService.CreateGuildsAsync(guildRequest);

            //Assert
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetGuildAsync_ThreeGuilds_ReturnsSuccessResponse()
        {
            //Arrange

            var guilds = new List<Guild>()
            {
                new Guild { Id = Guid.NewGuid(), Name = "Guild1", OwnerId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
                new Guild { Id = Guid.NewGuid(), Name = "Guild2", OwnerId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
                new Guild { Id = Guid.NewGuid(), Name = "Guild3", OwnerId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
            };

            _guildRepositoryMock
                .Setup(g => g.GetGuildsAsync())
                .ReturnsAsync(guilds);


            //Act
            var response = await _guildService.GetGuildsAsync();

            //Assert
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.Equal(3, response.Data?.Count());
        }

        [Fact]
        public async Task DeleteGuildAsync_GuildDeleted_ReturnsSuccessResponse()
        {
            //Arrange

            _guildRepositoryMock
                .Setup(g => g.DeleteGuildAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            //Act
            var response = await _guildService.DeleteGuildAsync(It.IsAny<Guid>(), It.IsAny<Guid>());

            //Assert
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetChannelsGuildAsync_ThreeChannels_ReturnsSuccessResponse()
        {
            //Arrange

            var channels = new List<Channel>()
            {
                new() {Id = Guid.NewGuid(), GuildId =  Guid.NewGuid() },
                new() {Id = Guid.NewGuid(), GuildId =  Guid.NewGuid() },
                new() {Id = Guid.NewGuid(), GuildId =  Guid.NewGuid() }
            };

            _guildRepositoryMock
                .Setup(g => g.GetChannelsGuildAsync(It.IsAny<Guid>()))
                .ReturnsAsync(channels);


            //Act
            var response = await _guildService.GetChannelsGuildAsync(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.Equal(3, response.Data?.Count());
        }

    }
}
