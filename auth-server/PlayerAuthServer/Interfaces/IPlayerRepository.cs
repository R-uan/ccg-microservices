using PlayerAuthServer.Models;

namespace PlayerAuthServer.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> SavePlayer(Player player);
        Task<Player?> FindPlayer(Guid Id);
        Task<Player?> FindPlayerByEmail(string email);
        Task<Player?> FindPlayerByUsername(string Username);
    }
}