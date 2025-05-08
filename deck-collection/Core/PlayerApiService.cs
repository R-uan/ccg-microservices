using System.Net.Http.Headers;
using DeckCollection.Models;

namespace DeckCollection.Core
{
    public class PlayerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public PlayerApiService(AppSettings appSettings)
        {
            this._httpClient = new();
            this._appSettings = appSettings;
            this._httpClient.BaseAddress = new Uri(this._appSettings.PlayerApiAddr);
        }

        public async Task GetPlayerInformation(string token, List<CardReference> cards)
        {
            this._httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var request = await this._httpClient.PostAsJsonAsync("/api/player/cards/check", cards);
            if (!request.IsSuccessStatusCode) throw new Exception("Whoops");
        }
    }
}