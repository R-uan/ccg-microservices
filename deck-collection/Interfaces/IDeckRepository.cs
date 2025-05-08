using DeckCollection.Models;

namespace DeckCollection.Interfaces
{
    public interface IDeckRepository
    {
        Task<List<Deck>> GetDecksAsync();
        Task<Deck> InsertDeck(Deck deck);
        Task<List<Deck>> GetPlayerDecksAsync(Guid playerId);
        Task<Deck> GetDeckByIdAsync(Guid deckId, Guid playerId);
    }
}
