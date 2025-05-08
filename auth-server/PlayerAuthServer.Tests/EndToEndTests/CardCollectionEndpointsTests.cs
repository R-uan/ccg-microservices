using FluentAssertions;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using PlayerAuthServer.Models;
using PlayerAuthServer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Models.Responses;

namespace PlayerAuthServer.Tests.EndToEndTests
{
    public class CardCollectionEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient client;
        private readonly CustomWebApplicationFactory factory;

        public CardCollectionEndpointsTests(CustomWebApplicationFactory factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient();
        }

        [Fact]
        public async Task PostCardCollectionEndpoint_ShouldReturnOk()
        {
            var authService = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IAuthService>();
            Assert.NotNull(authService);

            var login = new LoginRequest
            {
                Email = "tester1@protonmail.com",
                Password = "Tester1"
            };

            var token = await authService.AuthenticatePlayer(login);
            this.client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var collectCardRequest = new CollectCardRequest
            {
                CardId = Guid.NewGuid(),
                Amount = 2
            };

            var request = await this.client.PostAsJsonAsync("/api/player/collection", collectCardRequest);
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await request.Content.ReadFromJsonAsync<CardCollection>();
            response.Should().NotBeNull();
            response.Should().BeOfType<CardCollection>();
        }

        [Fact]
        public async Task PostCardCollectionEndpoint_ShouldReturnUnauthorized()
        {
            this.client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", null);

            var collectCardRequest = new CollectCardRequest
            {
                CardId = Guid.NewGuid(),
                Amount = 2
            };

            var request = await this.client.PostAsJsonAsync("/api/player/collection", collectCardRequest);
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CheckCardCollectionEndpoint_ShouldReturnOk()
        {
            using var scope = this.factory.Services.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            Assert.NotNull(authService);

            var login = new LoginRequest
            {
                Email = "tester1@protonmail.com",
                Password = "Tester1"
            };

            var token = await authService.AuthenticatePlayer(login);

            this.client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var collectCardRequest = new CollectCardRequest
            {
                CardId = Guid.NewGuid(),
                Amount = 2
            };

            var request = await this.client.PostAsJsonAsync("/api/player/collection", collectCardRequest);
            request.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await request.Content.ReadFromJsonAsync<CardCollection>();
            response.Should().NotBeNull();
            response.Should().BeOfType<CardCollection>();

            var checkCardRequest = new CheckCardCollectionRequest
            {
                CardIds = [collectCardRequest.CardId.ToString(), "invalid-guid", Guid.NewGuid().ToString()]
            };

            var checkRequest = await this.client.PostAsJsonAsync("/api/player/collection/check", checkCardRequest);
            checkRequest.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var checkResponse = await checkRequest.Content.ReadFromJsonAsync<CheckCollectionResponse>();
            checkResponse.Should().NotBeNull();
            checkResponse.Should().BeOfType<CheckCollectionResponse>();
            checkResponse.UnownedCards.Should().HaveCount(1);
            checkResponse.OwnedCards.Should().HaveCount(1);
            checkResponse.InvalidCards.Should().HaveCount(1);
        }

        [Fact]
        public async Task CheckCardCollectionEndpoint_ShouldReturnUnauthorized()
        {

            this.client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", null);

            var collectCardRequest = new CollectCardRequest
            {
                CardId = Guid.NewGuid(),
                Amount = 2
            };

            var checkCardRequest = new CheckCardCollectionRequest
            {
                CardIds = [collectCardRequest.CardId.ToString(), "invalid-guid", Guid.NewGuid().ToString()]
            };

            var checkRequest = await this.client.PostAsJsonAsync("/api/player/collection/check", checkCardRequest);
            checkRequest.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}