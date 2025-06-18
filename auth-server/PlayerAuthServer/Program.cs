using System.Text;
using PlayerAuthServer.Database;
using PlayerAuthServer.Utilities;
using PlayerAuthServer.Services;
using PlayerAuthServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using PlayerAuthServer.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlayerAuthServer.gRPC.Services;

namespace PlayerAuthServer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<PlayerDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PgDb");
                options.UseNpgsql(connectionString);
            });

            var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddScoped<ICardCollectionRepository, CardCollectionRepository>();
            builder.Services.AddScoped<ICardCollectionService, CardCollectionService>();

            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<TokenValidator>();

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>()
                              ?? throw new Exception("JwtSettings were not found on the configuration file.");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                IssuerSigningKey = signingKey,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
            };
            
            builder.Services.AddSingleton(validationParameters);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => { options.TokenValidationParameters = validationParameters; });

            var app = builder.Build();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.MapGrpcService<PlayerGrpcServiceImpl>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}