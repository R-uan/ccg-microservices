using CardCatalog;
using MongoDB.Driver;
using CardCatalog.Core;
using CardCatalog.Interface;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var mongoSettingsSection = builder.Configuration.GetSection("MongoSettings");
var mongoSettings = mongoSettingsSection.Get<MongoSettings>()
    ?? throw new Exception("Could not get MongoSettings from appsettings file.");

var client = new MongoClient(mongoSettings.URI);
var database = client.GetDatabase(mongoSettings.Database);
var cardCollection = database.GetCollection<Card>("cards");

builder.Services.AddSingleton<IMongoCollection<Card>>(cardCollection);
builder.Services.AddScoped<ICardCatalogService, CardCatalogService>();
builder.Services.AddScoped<ICardCatalogRepository, CardCollectionRepository>();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

