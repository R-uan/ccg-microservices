using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayerAuthServer.Models
{
    [ExcludeFromCodeCoverage]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }

        [JsonIgnore]
        public required string PasswordHash { get; set; }

        public List<CardCollection>? CardCollection { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }

        public DateTime LastLogin { get; set; }
        public bool IsBanned { get; set; }
    }
}
