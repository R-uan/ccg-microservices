using PlayerAuthServer.Models;

namespace PlayerAuthServer.Interfaces
{
    /// <summary>
    /// Provides functionality for generating JSON Web Tokens (JWT) for authenticated players.
    /// </summary>
    /// <param name="jwtSettings">Injected JWT settings containing issuer and signing key.</param>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a signed JWT for the given player containing basic identity claims.
        /// </summary>
        /// <param name="player">The authenticated player to generate the token for.</param>
        /// <returns>A JWT string signed with the configured HMAC key.</returns>
        string GenerateToken(Player player);
    }
}