using DeckCollection.Models;

namespace DeckCollection.Interfaces
{
    public interface IDeckService
    {
        Task<Deck> GetDeckAsync(Guid playerId, Guid deckId);
        Task<List<Deck>> GetPlayerDecksAsync(Guid playerId);
        Task<Deck> CreateDeckAsync(CreateDeckRequest request, Guid playerId);
    }
}