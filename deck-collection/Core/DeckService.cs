using DeckCollection.Models;
using DeckCollection.Interfaces;

namespace DeckCollection.Core
{
    public class DeckService(IDeckRepository deckRepository) : IDeckService
    {
        public async Task<Deck> CreateDeckAsync(CreateDeckRequest request, Guid playerId)
        {
            Deck newDeck = new()
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId,
                Name = request.Name,
                Cards = request.Cards,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var insert = await deckRepository.InsertDeck(newDeck);

            return insert;
        }

        public async Task<List<Deck>> GetPlayerDecksAsync(Guid playerId)
            => await deckRepository.GetPlayerDecksAsync(playerId);

        public async Task<Deck> GetDeckAsync(Guid playerId, Guid deckId)
            => await deckRepository.GetDeckByIdAsync(deckId, playerId);
    }
}
