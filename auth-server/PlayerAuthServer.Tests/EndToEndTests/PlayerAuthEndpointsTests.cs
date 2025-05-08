using FluentAssertions;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Models.Responses;
using System.Net.Http.Json;

namespace PlayerAuthServer.Tests.EndToEndTests;

public class PlayerAuthEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnRegisterResponse()
    {
        var request = new RegisterRequest
        {
            Email = "tester@protonmail.com",
            Username = "Tester",
            Password = "besttesteroutthere"
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();

        Assert.NotNull(result);
        Assert.Equal(request.Username, result.Player.Username);
    }

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnBadRequestUsername()
    {
        var request = new RegisterRequest
        {
            Email = "tester23@protonmail.com",
            Username = "Tester1",
            Password = "besttesteroutthere"
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnBadRequestEmail()
    {
        var request = new RegisterRequest
        {
            Email = "tester1@protonmail.com",
            Username = "Tester32",
            Password = "besttesteroutthere"
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task LoginEndpoint_ShouldReturnLoginResponse()
    {
        var request = new LoginRequest
        {
            Email = "tester1@protonmail.com",
            Password = "Tester1"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginEndpoint_ShouldReturnUnauthorizedEmail()
    {
        var request = new LoginRequest
        {
            Email = "tester1@wrongmail.com",
            Password = "Tester1"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LoginEndpoint_ShouldReturnUnauthorizedPassword()
    {
        var request = new LoginRequest
        {
            Email = "tester1@protonmail.com",
            Password = "wrongpassword"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}

