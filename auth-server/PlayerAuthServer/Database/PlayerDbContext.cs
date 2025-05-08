using PlayerAuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace PlayerAuthServer.Database
{
    public class PlayerDbContext(DbContextOptions<PlayerDbContext> options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<CardCollection> CardCollection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(player =>
            {
                player.ToTable("players");
                player.Property(p => p.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
                player.HasKey(p => p.Id);
                player.HasMany(p => p.CardCollection)
                    .WithOne(d => d.Player)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CardCollection>(cards =>
            {
                cards.ToTable("card_collection");
                cards.HasKey(card => new { card.PlayerId, card.CardId });
                cards.Property(card => card.CardId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}