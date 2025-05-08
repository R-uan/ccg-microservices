using Moq;
using PlayerAuthServer.Services;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Utilities.Exceptions;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Tests.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService authService;
        private readonly Mock<IJwtService> mockJwtService;
        private readonly Mock<IPlayerService> mockPlayerService;
        private readonly Mock<IPlayerRepository> mockPlayerRepository;

        public AuthServiceTests()
        {
            this.mockJwtService = new Mock<IJwtService>();
            this.mockPlayerService = new Mock<IPlayerService>();
            this.mockPlayerRepository = new Mock<IPlayerRepository>();
            this.authService = new AuthService(
                this.mockJwtService.Object,
                this.mockPlayerService.Object,
                this.mockPlayerRepository.Object
            );
        }

        [Fact]
        public async Task AuthenticatePlayer_ShouldReturnTokenString()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("hash1");
            LoginRequest login = new()
            {
                Email = "player1@example.com",
                Password = "hash1"
            };

            Player player = new()
            {
                Email = "player1@example.com",
                Username = "PlayerOne",
                PasswordHash = hashedPassword,
            };

            this.mockPlayerRepository.Setup(repository => repository.FindPlayerByEmail(login.Email))
                .ReturnsAsync((Player?)player);
            this.mockJwtService.Setup(service => service.GenerateToken(player))
                .Returns("It.IsAny<string>()");

            string result = await authService.AuthenticatePlayer(login);

            this.mockPlayerRepository.Verify(repository => repository.FindPlayerByEmail(It.IsAny<string>()), Times.Once);
            this.mockJwtService.Verify(service => service.GenerateToken(It.IsAny<Player>()), Times.Once);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AuthenticatePlayer_ShouldThrowEmailNotFoundException()
        {
            LoginRequest login = new()
            {
                Email = "player1@example.com",
                Password = "hash1"
            };

            this.mockPlayerRepository.Setup(repository => repository.FindPlayerByEmail(login.Email))
                .ReturnsAsync((Player?)null);

            var result = await Assert.ThrowsAsync<PlayerNotFoundException>(
                async () => await this.authService.AuthenticatePlayer(login)
            );

            Assert.IsType<PlayerNotFoundException>(result);
        }


        [Fact]
        public async Task AuthenticatePlayer_ShouldThrowUnauthorizedAccessException()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("hash1");
            LoginRequest login = new()
            {
                Email = "player1@example.com",
                Password = "hash2"
            };

            Player player = new()
            {
                Email = "player1@example.com",
                Username = "PlayerOne",
                PasswordHash = hashedPassword,
            };

            this.mockPlayerRepository.Setup(repository => repository.FindPlayerByEmail(login.Email))
                .ReturnsAsync((Player?)player);

            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                async () => await authService.AuthenticatePlayer(login)
            );

            this.mockPlayerRepository.Verify(repository => repository.FindPlayerByEmail(It.IsAny<string>()), Times.Once);
            Assert.IsType<UnauthorizedAccessException>(result);
        }

        [Fact]
        public async Task RegisterNewPlayer_ShouldReturnPartialPlayerProfile()
        {
            Player player = new()
            {
                Email = "email@gmail.com",
                Username = "hotmail",
                PasswordHash = "protonmail",
            };

            RegisterRequest registerRequest = new()
            {
                Email = "email@gmail.com",
                Username = "hotmail",
                Password = "protonmail"
            };

            var newPlayer = NewPlayer.Create(registerRequest);

            this.mockPlayerService.Setup(service => service.CreatePlayerWithDefaults(It.IsAny<NewPlayer>()))
                .ReturnsAsync(player);

            var result = await this.authService.RegisterNewPlayer(registerRequest);

            Assert.NotNull(result);

        }
    }
}