using Moq;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;
using PlayerAuthServer.Services;

namespace PlayerAuthServer.Tests.UnitTests.Services
{
    public class PlayerServiceTest
    {
        private readonly IPlayerService playerService;
        private readonly Mock<IPlayerRepository> mockPlayerRepository;

        public PlayerServiceTest()
        {
            this.mockPlayerRepository = new Mock<IPlayerRepository>();
            this.playerService = new PlayerService(this.mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetPartialPlayerProfileAsyn_ShouldReturnPartialPlayerProfile()
        {
            var mockPlayer = new Player { Email = "tes", PasswordHash = "tes", Username = "tes" };
            this.mockPlayerRepository.Setup(x => x.FindPlayer(It.IsAny<Guid>()))
                .ReturnsAsync(mockPlayer);

            var result = await this.playerService.GetPartialPlayerProfileAsync(Guid.NewGuid());
            Assert.NotNull(result);
            Assert.IsType<PartialPlayerProfile>(result);
        }

        [Fact]
        public async Task GetPartialPlayerProfileAsync_ShouldReturnNull()
        {
            this.mockPlayerRepository.Setup(x => x.FindPlayer(It.IsAny<Guid>()))
                .ReturnsAsync((Player?)null);

            var result = await this.playerService.GetPartialPlayerProfileAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task CreatePlayerWithDefaults_ShouldReturnPlayerEntity()
        {

            this.mockPlayerRepository.Setup(p => p.SavePlayer(It.IsAny<Player>()))
                .ReturnsAsync(new Player { Email = "tes", PasswordHash = "tes", Username = "tes" });


            var newPlayer = new NewPlayer { Email = "tes", PasswordHash = "tes", Username = "tes" };
            var result = await this.playerService.CreatePlayerWithDefaults(newPlayer);

            Assert.NotNull(result);
            Assert.IsType<Player>(result);
        }
    }
}