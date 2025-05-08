using MongoDB.Driver;
using DeckCollection.Models;
using DeckCollection.Interfaces;

namespace DeckCollection.Core
{
    public class DeckRepository(IMongoCollection<Deck> deckCollection) : IDeckRepository
    {
        public async Task<List<Deck>> GetPlayerDecksAsync(Guid playerId)
        {
            var decks = await deckCollection.FindAsync(deck => deck.PlayerId == playerId);
            return await decks.ToListAsync();
        }

        public async Task<List<Deck>> GetDecksAsync()
        {
            var decks = await deckCollection.FindAsync(_ => true);
            return await decks.ToListAsync();
        }

        public async Task<Deck> InsertDeck(Deck deck)
        {
            await deckCollection.InsertOneAsync(deck);
            return await deckCollection.FindAsync(d => d.Id == deck.Id).Result.FirstAsync()
                ?? throw new Exception("Could not save Deck");
        }

        public async Task<Deck> GetDeckByIdAsync(Guid deckId, Guid playerId)
        {
            var deck = await deckCollection.FindAsync(deck => deck.Id == deckId && deck.PlayerId == playerId);
            return await deck.FirstAsync();
        }
    }
}
