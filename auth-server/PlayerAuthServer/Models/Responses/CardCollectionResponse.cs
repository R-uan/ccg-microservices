namespace PlayerAuthServer.Models.Responses
{
    public record CheckCollectionResponse(List<CardCollection> OwnedCards, List<Guid> UnownedCards, List<string> InvalidCards);
}