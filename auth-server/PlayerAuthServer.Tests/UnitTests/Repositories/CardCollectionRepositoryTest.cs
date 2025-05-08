using System;
using Microsoft.EntityFrameworkCore;
using PlayerAuthServer.Database;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;
using PlayerAuthServer.Repositories;

namespace PlayerAuthServer.Tests.UnitTests.Repositories
{
    public class CardCollectionRepositoryTest
    {
        private readonly List<Guid> PlayerCards;
        private readonly Guid PlayerTestId = Guid.NewGuid();

        private readonly PlayerDbContext playerDbContext;
        private readonly ICardCollectionRepository cardCollectionRepository;

        public CardCollectionRepositoryTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<PlayerDbContext>()
                .UseInMemoryDatabase(databaseName: "cardCollectionTest").Options;

            this.playerDbContext = new PlayerDbContext(dbContextOptions);
            this.cardCollectionRepository = new CardCollectionRepository(this.playerDbContext);


            var cardCollection = new List<CardCollection>
        {
            new(Guid.NewGuid(), 2, this.PlayerTestId),
            new(Guid.NewGuid(), 2, this.PlayerTestId),
            new(Guid.NewGuid(), 2, Guid.NewGuid()),
            new(Guid.NewGuid(), 2, Guid.NewGuid()),
            new(Guid.NewGuid(), 2, Guid.NewGuid()),
        };
            this.PlayerCards = [cardCollection[0].CardId, cardCollection[1].CardId];
            this.playerDbContext.CardCollection.AddRange(cardCollection);
            this.playerDbContext.SaveChanges();
        }

        [Fact]
        public async Task SaveEntity_ShouldReturnCardCollectionEntityTest()
        {
            var newEntity = new CardCollection(Guid.NewGuid(), 5, Guid.NewGuid());
            var saveEntity = await this.cardCollectionRepository.SaveEntity(newEntity);
            Assert.NotNull(saveEntity);
            Assert.Equal(newEntity, saveEntity);
        }

        [Fact]
        public async Task FindPlayerCardCollection_ShouldReturnListOfTwoCardCollection()
        {
            var findCardCollection = await this.cardCollectionRepository.FindPlayerCardCollection(this.PlayerTestId);
            Assert.NotNull(findCardCollection);
            Assert.Equal(2, findCardCollection.Count);
        }

        [Fact]
        public async Task FindOwnedCards_ShouldReturnListOfTwoCardCollection()
        {
            var findOwnedCards = await this.cardCollectionRepository.FindOwnedCards(this.PlayerCards, this.PlayerTestId);
            Assert.NotNull(findOwnedCards);
            Assert.Equal(2, findOwnedCards.Count);
        }
    }
}