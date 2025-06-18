using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PlayerAuthServer.Utilities;

public class TokenValidator(TokenValidationParameters validationParameters)
{
    private readonly JwtSecurityTokenHandler tokenHandler = new();

    public ClaimsPrincipal? ValidateToken(string token)
            => this.tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
}