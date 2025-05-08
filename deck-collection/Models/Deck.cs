using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeckCollection.Models
{
    public class Deck
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public required Guid PlayerId { get; set; }

        public required string Name { get; set; }
        public required List<CardReference> Cards { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
