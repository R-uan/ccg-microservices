using Moq;
using PlayerAuthServer.Utilities;
using Microsoft.Extensions.Options;
using PlayerAuthServer.Services;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Tests.UnitTests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService jwtService;
        private readonly Mock<IOptions<JwtSettings>> mockJwtSettings;

        public JwtServiceTests()
        {
            var settings = new JwtSettings()
            {
                Issuer = "Tester",
                SigningKey = "A3F1B2C490D5E678F12A34B5C6D7E890"
            };

            this.mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            this.mockJwtSettings.Setup(settings => settings.Value).Returns(settings);
            this.jwtService = new JwtService(this.mockJwtSettings.Object);
        }

        [Fact]
        public void ShouldGenerateJwToken()
        {
            Player player = new()
            {
                Email = "player1@example.com",
                Username = "PlayerOne",
                PasswordHash = "hash1",
            };

            var result = this.jwtService.GenerateToken(player);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}