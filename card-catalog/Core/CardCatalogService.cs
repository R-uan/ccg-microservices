using MongoDB.Driver;
using CardCatalog.Models;
using CardCatalog.Interface;

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

            return new SelectedCardsResponse(cards, invalidGuids, notFound);
        }

        public async Task<List<Card>> QueryCards(SearchQuery query)
        {
            var filterBuilder = Builders<Card>.Filter;
            var filter = FilterDefinition<Card>.Empty;

            if (query.Categories?.Count > 0)
                filter &= filterBuilder.In(c => c.Category, query.Categories);
            if (query.Attack?.Count > 0)
                filter &= filterBuilder.In(c => c.Attack, query.Attack);
            if (query.Health?.Count > 0)
                filter &= filterBuilder.In(c => c.Health, query.Health);
            if (query.ManaCost?.Count > 0)
                filter &= filterBuilder.In(c => c.ManaCost, query.ManaCost);
            if (query.Rarity?.Count > 0)
                filter &= filterBuilder.In(c => c.Health, query.Health);

            return await cardCatalogRepository.FindCards(filter);
        }
    }
}