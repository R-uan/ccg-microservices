using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Services
{
    public class PlayerService(IPlayerRepository playerRepository) : IPlayerService
    {
        public async Task<Player> CreatePlayerWithDefaults(NewPlayer newPlayer)
        {
            var player = new Player
            {
                Email = newPlayer.Email,
                Username = newPlayer.Username,
                PasswordHash = newPlayer.PasswordHash,
            };

            // Need to add default decks (I'll do it later chill)

            var createdPlayer = await playerRepository.SavePlayer(player);
            return createdPlayer;
        }

        public async Task<AuthenticatedPlayer?> AuthenticatePlayerIdentity(Guid playerId)
        {
            var player = await playerRepository.FindPlayer(playerId);
            return player == null ? null : AuthenticatedPlayer.Create(player);
        }
    }
}
