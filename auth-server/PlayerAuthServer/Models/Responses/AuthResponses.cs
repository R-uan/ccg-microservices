namespace PlayerAuthServer.Models.Responses
{
    public record LoginResponse(string Token);
    public record RegisterResponse(PartialPlayerProfile Player);
}
