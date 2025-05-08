using System.Text;
using System.Security.Claims;
using PlayerAuthServer.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Services
{
    public class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public string GenerateToken(Player player)
        {
            byte[] signingKey = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = signingCredentials,
                Subject = GenerateIdentity(player),
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> with claims for the player’s email and ID.
        /// </summary>
        /// <param name="player">The player entity to extract claims from.</param>
        /// <returns>A claims identity used for token generation.</returns>
        private static ClaimsIdentity GenerateIdentity(Player player)
        {
            var claimIdentity = new ClaimsIdentity();
            var emailClaim = new Claim(ClaimTypes.Email, player.Email);
            var IdClaim = new Claim("Id", player.Id.ToString());
            claimIdentity.AddClaims([emailClaim, IdClaim]);
            return claimIdentity;
        }
    }
}