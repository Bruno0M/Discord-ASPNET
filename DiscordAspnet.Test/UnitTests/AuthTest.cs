using DiscordAspnet.Application.DTOs.UserDTOs;
using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Application.Services;
using DiscordAspnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DiscordAspnet.Test.AuthTests
{
    public class AuthTest
    {

        [Fact]
        public void Should_Create_User_With_Encrypted_Password()
        {
            //Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            var mockTokenRepository = new Mock<ITokenService>();
            var authService = new AuthService(mockUserManager.Object, mockTokenRepository.Object);

            var user = new UserRequest(
                "Test",
                "test@test.com",
                "test123",
                "test123");

            //Act
            var response = authService.Register(user);


            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Login_With_Valid_Credentials_Should_Return_Access_Token()
        {
            //Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            var mockTokenRepository = new Mock<ITokenService>();

            var expectedUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "Test",
                Email = "Test@test.com",
                CreatedAt = DateTime.Now
            };

            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(expectedUser));

            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
               .Returns(Task.FromResult(true));

            var expectedToken = "ThisIsAValidAccessToken";
            mockTokenRepository.Setup(tr => tr.GenerateToken(It.IsAny<ApplicationUser>()))
                               .Returns(expectedToken);

            var authService = new AuthService(mockUserManager.Object, mockTokenRepository.Object);

            var loginRequest = new LoginRequest(
                Credential: "Test@test.com",
                Password: "test123");

            //Act
            var response = await authService.Login(loginRequest);
            var token = response.Data?.Token;


            //Assert
            Assert.NotNull(token);
            Assert.Equal(expectedToken, token);
        }
    }
}
