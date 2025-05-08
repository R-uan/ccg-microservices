namespace CardCatalog.Interface
{
    public interface ICardCatalogRepository
    {
        Task<List<Card>> FindCards();
        Task<Card?> FindCard(Guid id);
        Task<Card?> SaveCard(Card card);
        Task<int> SaveMultipleCards(List<Card> cards);
        Task<List<Card>> FindCards(List<Guid> cardIds);
    }
}
