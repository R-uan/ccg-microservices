namespace PlayerAuthServer.Models
{
    public class PlayerProfile
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }

        public List<CardCollection>? CardCollection { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }

        public DateTime LastLogin { get; set; }
        public bool IsBanned { get; set; }

        public static PlayerProfile Create(Player player)
            => new PlayerProfile
            {
                Id = player.Id,
                Email = player.Email,
                Username = player.Username,
                CardCollection = player.CardCollection,
                Level = player.Level,
                Experience = player.Experience,
                Wins = player.Wins,
                Losses = player.Losses,
                LastLogin = player.LastLogin,
                IsBanned = player.IsBanned,
            };

    }
}