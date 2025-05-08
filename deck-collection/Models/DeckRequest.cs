namespace DeckCollection.Models
{
    public record CreateDeckRequest(string Name, List<CardReference> Cards);
}
