namespace PlayerAuthServer.Models.Requests
{
    public class CollectCardRequest
    {
        public required Guid CardId { get; set; }
        public required int Amount { get; set; }

        public CollectCardRequest() { }
        public CollectCardRequest(Guid cardId, int amount)
        {
            this.CardId = cardId;
            this.Amount = amount;
        }
    }

    public class CheckCardCollectionRequest
    {
        public required List<string> CardIds { get; set; }

        public CheckCardCollectionRequest() { }
        public CheckCardCollectionRequest(List<string> cardIds) { this.CardIds = cardIds; }
    }
}
