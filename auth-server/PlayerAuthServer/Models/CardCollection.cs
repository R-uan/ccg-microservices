using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayerAuthServer.Models
{
    public class CardCollection(Guid cardId, int amount, Guid playerId)
    {
        [Key]
        public Guid CardId { get; set; } = cardId;
        public int Amount { get; set; } = amount;

        [Key]
        [JsonIgnore]
        public Guid PlayerId { get; set; } = playerId;

        [JsonIgnore]
        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }
    }
}