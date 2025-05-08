namespace CardCatalog.Models
{
    public class SelectedCardsRequest
    {
        public required List<string> CardIds { get; set; }
    }
    public class SelectedCardsResponse(List<Card> Cards, List<string> InvalidCardGuid, List<Guid> CardsNotFound);
}