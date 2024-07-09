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
        public async Task Should_Creat_Guild()
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
        public async Task Should_Get_All_Guilds()
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
    }
}
