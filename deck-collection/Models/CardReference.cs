using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeckCollection.Models
{
    /// <summary>
    /// Represents a reference to a card in the player's inventory or collection.
    /// Stores the card's unique identifier and the quantity of that card the player possesses.
    /// </summary>
    public class CardReference
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
}
