using PlayerAuthServer.Models;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models.Responses;
using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Services
{
    public class CardCollectionService(ICardCollectionRepository cardRepository, IPlayerRepository playerRepository) : ICardCollectionService
    {
        public async Task<CheckCollectionResponse> CheckCollection(CheckCardCollectionRequest request, Guid playerId)
        {
            List<Guid> validGuids = [];
            List<string> invalidGuids = [];

            request.CardIds.ForEach(guid => { if (Guid.TryParse(guid, out var valid)) validGuids.Add(valid); else invalidGuids.Add(guid); });
            var ownedCards = await cardRepository.FindOwnedCards(validGuids, playerId);
            var notFound = validGuids.Where(valid => !ownedCards.Any(x => x.CardId == valid)).ToList();
            var response = new CheckCollectionResponse(ownedCards, notFound, invalidGuids);
            return response;
        }

        public async Task<CardCollection> CollectCard(CollectCardRequest request, Guid playerId)
        {
            var newAcquisition = new CardCollection(request.CardId, request.Amount, playerId);
            var saveAcquisiton = await cardRepository.SaveEntity(newAcquisition);
            return saveAcquisiton;
        }

        public async Task<List<CardCollection>?> FindPlayerCollection(Guid playerId)
        {
            if (await playerRepository.FindPlayer(playerId) == null) return null;
            var cardCollection = await cardRepository.FindPlayerCardCollection(playerId);
            return cardCollection;
        }
    }
}