namespace CardCatalog.Models
{
    public class SelectedCardsResponse
    {
        public required List<Card> Cards { get; set; }
        public required List<string> InvalidCardGuid { get; set; }
        public required List<Guid> CardsNotFound { get; set; }
    }
}