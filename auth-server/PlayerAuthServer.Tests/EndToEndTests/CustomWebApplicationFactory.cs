using PlayerAuthServer.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Tests.EndToEndTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PlayerDbContext>));
                if (dbDescriptor != null) services.Remove(dbDescriptor);
                services.AddDbContext<PlayerDbContext>(options => options.UseInMemoryDatabase("E2ETest"));
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PlayerDbContext>();
                this.SeedPlayerData(db);
            });

            return base.CreateHost(builder);
        }

        private void SeedPlayerData(PlayerDbContext playerDbContext)
        {
            playerDbContext.Players.AddRange(
                new Player
                {
                    Email = "tester1@protonmail.com",
                    Username = "tester1",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tester1")
                },
                new Player
                {
                    Email = "tester2@protonmail.com",
                    Username = "tester2",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tester2")
                }
            );

            playerDbContext.SaveChanges();
        }
    }

}