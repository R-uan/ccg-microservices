using System.Text;
using MongoDB.Driver;
using DeckCollection.Core;
using DeckCollection.Models;
using DeckCollection.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var mongoSettingsSection = builder.Configuration.GetSection("MongoSettings");
var mongoSettings = mongoSettingsSection.Get<MongoSettings>()
    ?? throw new Exception("Could not get MongoSettings from appsettings file.");

var mongoDbClient = new MongoClient(mongoSettings.URI);
var targetDatabase = mongoDbClient.GetDatabase(mongoSettings.Database);
var deckCollection = targetDatabase.GetCollection<Deck>("decks");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
    var jwtSettings = jwtSettingsSection.Get<JwtSettings>()
                    ?? throw new Exception("JwtSettings were not found on the configuration file.");

    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
    };
});

builder.Services.AddSingleton<IMongoCollection<Deck>>(deckCollection);
builder.Services.AddScoped<IDeckRepository, DeckRepository>();
builder.Services.AddScoped<IDeckService, DeckService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
