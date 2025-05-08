using Microsoft.EntityFrameworkCore;
using PlayerAuthServer.Database;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Repositories
{
    public class PlayerRepository(PlayerDbContext dbContext) : IPlayerRepository
    {
        public async Task<Player> SavePlayer(Player player)
        {
            var entry = await dbContext.Players.AddAsync(player);
            int affectedRows = await dbContext.SaveChangesAsync();
            if (affectedRows <= 0) throw new DbUpdateException("Could not insert Player entity.");
            return entry.Entity;
        }

        public async Task<Player?> FindPlayer(Guid Id)
            => await dbContext.Players
                        .Include(p => p.CardCollection)
                        .FirstOrDefaultAsync(p => p.Id == Id);

        public async Task<Player?> FindPlayerByEmail(string email)
                    => await (from p in dbContext.Players
                              where p.Email.ToLower() == email.ToLower()
                              select p).FirstOrDefaultAsync();


        public async Task<Player?> FindPlayerByUsername(string Username)
                    => await (from p in dbContext.Players
                              where p.Username.ToLower() == Username.ToLower()
                              select p).FirstOrDefaultAsync();
    }
}