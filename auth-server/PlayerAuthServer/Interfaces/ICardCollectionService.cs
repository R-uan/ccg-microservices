using PlayerAuthServer.Models;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Models.Responses;

namespace PlayerAuthServer.Interfaces
{
    public interface ICardCollectionService
    {
        Task<List<CardCollection>?> FindPlayerCollection(Guid playerId);
        Task<CardCollection> CollectCard(CollectCardRequest request, Guid playerId);
        Task<CheckCollectionResponse> CheckCollection(CheckCardCollectionRequest request, Guid playerId);
    }
}
