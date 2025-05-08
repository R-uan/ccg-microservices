namespace PlayerAuthServer.Models
{
    public class PartialPlayerProfile
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }

        public int Level { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }


        public static PartialPlayerProfile Create(Player player)
        {
            return new PartialPlayerProfile
            {
                Id = player.Id,
                Username = player.Username,
                Level = player.Level,
                Losses = player.Losses,
                Wins = player.Wins
            };
        }
    }
}
