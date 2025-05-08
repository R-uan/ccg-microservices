namespace PlayerAuthServer.Utilities
{
    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string SigningKey { get; set; }
    }
}
