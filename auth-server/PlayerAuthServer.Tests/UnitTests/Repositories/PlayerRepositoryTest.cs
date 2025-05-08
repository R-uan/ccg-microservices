using PlayerAuthServer.Models;
using PlayerAuthServer.Database;
using PlayerAuthServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using PlayerAuthServer.Repositories;

namespace PlayerAuthServer.Tests.UnitTests.Repositories
{
    public class PlayerRepositoryTest
    {
        private readonly PlayerDbContext playerDbContext;
        private readonly IPlayerRepository playerRepository;

        public PlayerRepositoryTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<PlayerDbContext>()
                .UseInMemoryDatabase(databaseName: "playerTestDatabase").Options;
            this.playerDbContext = new PlayerDbContext(dbContextOptions);
            this.playerRepository = new PlayerRepository(this.playerDbContext);

            var players = new List<Player>
            {
                new() { Email = "player1@example.com", Username = "PlayerOne", PasswordHash = "hash1",  },
                new() { Email = "player2@example.com", Username = "PlayerTwo", PasswordHash = "hash2",  },
                new() { Email = "player3@example.com", Username = "PlayerThree", PasswordHash = "hash3",  },
                new() { Email = "player4@example.com", Username = "PlayerFour", PasswordHash = "hash4",  },
                new() { Email = "player5@example.com", Username = "PlayerFive", PasswordHash = "hash5",  }
            };

            this.playerDbContext.Players.AddRange(players);
            this.playerDbContext.SaveChanges();
        }

        [Fact]
        public async Task ShouldFindPlayerByUsername()
        {
            var player = await this.playerRepository.FindPlayerByUsername("playerone");
            Assert.NotNull(player);
        }

        [Fact]
        public async Task ShouldNotFindPlayerByUsername()
        {
            var player = await this.playerRepository.FindPlayerByUsername("notexist");
            Assert.Null(player);
        }

        [Fact]
        public async Task ShouldFindPlayerByEmail()
        {
            var player = await this.playerRepository.FindPlayerByEmail("player4@example.com");
            Assert.NotNull(player);
        }

        [Fact]
        public async Task ShouldNotFindPlayerByEmail()
        {
            var player = await this.playerRepository.FindPlayerByEmail("notexist@coldmail.com");
            Assert.Null(player);
        }

        [Fact]
        public async Task ShouldFindPlayerByGuid()
        {
            Player newPlayer = new() { Email = "wen@wen.wen", Username = "wen", PasswordHash = "wen" };
            var playerEntity = await this.playerDbContext.Players.AddAsync(newPlayer);
            await this.playerDbContext.SaveChangesAsync();
            var playerId = playerEntity.Entity.Id;
            var player = await this.playerRepository.FindPlayer(playerId);
            Assert.NotNull(player);
        }

        [Fact]
        public async Task ShouldNotFindPlayerByGuid()
        {
            var player = await this.playerRepository.FindPlayer(Guid.NewGuid());
            Assert.Null(player);
        }

        [Fact]
        public async Task ShouldCreatePlayerEntity()
        {
            Player newPlayer = new() { Email = "new@new.new", Username = "new", PasswordHash = "wen" };
            var player = await this.playerRepository.SavePlayer(newPlayer);
            Assert.NotNull(player);
            Assert.IsType<Player>(player);
        }
    }
}
