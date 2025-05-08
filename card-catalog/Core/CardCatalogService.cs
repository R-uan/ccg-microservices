using CardCatalog.Interface;
using CardCatalog.Models;

namespace CardCatalog.Core
{
    public class CardCatalogService(ICardCatalogRepository cardCatalogRepository) : ICardCatalogService
    {
        public async Task<SelectedCardsResponse> SelectedCards(SelectedCardsRequest request)
        {
            List<Guid> notFound = [];
            List<Guid> validGuids = [];
            List<string> invalidGuids = [];

            request.CardIds.ForEach(id =>
            {
                var parse = Guid.TryParse(id, out var validGuid);
                if (!parse) invalidGuids.Add(id);
                else validGuids.Add(validGuid);
            });

            var cards = await cardCatalogRepository.FindCards(validGuids);
            validGuids.ForEach(validGuid =>
            {
                var exists = cards.Find(card => card.Id == validGuid);
                if (exists == null) notFound.Add(validGuid);
            });

            return new SelectedCardsResponse
            {
                Cards = cards,
                CardsNotFound = notFound,
                InvalidCardGuid = invalidGuids
            };
        }
    }
}