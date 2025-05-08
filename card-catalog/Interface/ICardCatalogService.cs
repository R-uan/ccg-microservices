using CardCatalog.Models;

namespace CardCatalog.Interface
{
    public interface ICardCatalogService
    {
        Task<SelectedCardsResponse> SelectedCards(SelectedCardsRequest request);
        Task<List<Card>> QueryCards(SearchQuery query);
    }
}