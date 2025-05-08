using Moq;
using PlayerAuthServer.Models;
using PlayerAuthServer.Services;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Tests.UnitTests.Services
{
    public class CardCollectionServiceTest
    {
        private readonly ICardCollectionService cardCollectionService;
        private readonly Mock<IPlayerRepository> mockPlayerRepository;
        private readonly Mock<ICardCollectionRepository> mockCardCollectionRepository;

        public CardCollectionServiceTest()
        {
            this.mockPlayerRepository = new Mock<IPlayerRepository>();
            this.mockCardCollectionRepository = new Mock<ICardCollectionRepository>();
            this.cardCollectionService = new CardCollectionService(
                this.mockCardCollectionRepository.Object,
                this.mockPlayerRepository.Object
            );
        }

        [Fact]
        public async Task CollectCard_ShouldReturnCardCollectionEntity()
        {
            var collectCard = new CollectCardRequest
            {
                Amount = 2,
                CardId = Guid.NewGuid()
            };

            var cardCollection = new CardCollection(Guid.NewGuid(), 2, Guid.NewGuid());
            this.mockCardCollectionRepository.Setup(c => c.SaveEntity(It.IsAny<CardCollection>()))
                .ReturnsAsync(cardCollection);

            var result = await this.cardCollectionService.CollectCard(collectCard, Guid.NewGuid());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindPlayerCollection_ShouldReturnListOfCardCollection()
        {
            var player = new Player { Id = Guid.NewGuid(), Email = "", PasswordHash = "", Username = "" };
            List<CardCollection> cardCollection = [
                new CardCollection(Guid.NewGuid(), 2, player.Id),
                new CardCollection(Guid.NewGuid(), 2, player.Id)
            ];

            this.mockPlayerRepository.Setup(p => p.FindPlayer(It.IsAny<Guid>()))
                .ReturnsAsync(player);

            this.mockCardCollectionRepository.Setup(c => c.FindPlayerCardCollection(It.IsAny<Guid>()))
                .ReturnsAsync(cardCollection);

            var result = await this.cardCollectionService.FindPlayerCollection(player.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindPlayerCollection_ShouldReturnNull()
        {
            this.mockPlayerRepository.Setup(p => p.FindPlayer(It.IsAny<Guid>()))
                .ReturnsAsync((Player?)null);
            var result = await this.cardCollectionService.FindPlayerCollection(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckCollection_ShouldReturnCollectionResponse()
        {
            var mockPlayerId = Guid.NewGuid();
            var mockReturn = new List<CardCollection> { new(Guid.NewGuid(), 1, mockPlayerId) };
            var request = new CheckCardCollectionRequest
            {
                CardIds = [mockReturn[0].CardId.ToString(), "invalid-one", Guid.NewGuid().ToString()]
            };

            this.mockCardCollectionRepository.Setup(repo => repo.FindOwnedCards(It.IsAny<List<Guid>>(), It.IsAny<Guid>()))
                .ReturnsAsync(mockReturn);

            var result = await this.cardCollectionService.CheckCollection(request, mockPlayerId);

            Assert.NotNull(result);
            Assert.Single(result.OwnedCards);
            Assert.Single(result.UnownedCards);
            Assert.Single(result.InvalidCards);
        }
    }
}