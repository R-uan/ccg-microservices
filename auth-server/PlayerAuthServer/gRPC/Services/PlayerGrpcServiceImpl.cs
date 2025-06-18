using Grpc.Core;
using PlayerAuthServer.GrpcServices;
using PlayerAuthServer.Services;
using PlayerAuthServer.Utilities;

namespace PlayerAuthServer.gRPC.Services;

public class PlayerGrpcServiceImpl(PlayerService playerService, TokenValidator tokenValidator)
    : GrpcServices.PlayerGrpcService.PlayerGrpcServiceBase
{
    public override async Task<AuthenticatedPlayer> AuthenticatePlayer(AuthenticationRequest request,
        ServerCallContext context)
    {
        var claims = tokenValidator.ValidateToken(request.PlayerToken);
        if (claims == null)
            return new PlayerAuthServer.GrpcServices.AuthenticatedPlayer()
                { Authenticated = false, IsBanned = false, Username = "", PlayerId = "" };

        var playerIdClaim = claims.FindFirst("playerId");
        if (playerIdClaim == null || !Guid.TryParse(playerIdClaim.Value, out var playerId))
            return new PlayerAuthServer.GrpcServices.AuthenticatedPlayer() { Authenticated = false };

        var authentication = await playerService.AuthenticatePlayerIdentity(playerId);
        return authentication != null
            ? new PlayerAuthServer.GrpcServices.AuthenticatedPlayer()
            {
                Authenticated = true,
                IsBanned = authentication.IsBanned,
                Username = authentication.Username,
                PlayerId = authentication.PlayerId.ToString()
            }
            : new PlayerAuthServer.GrpcServices.AuthenticatedPlayer()
                { Authenticated = false, IsBanned = false, Username = "", PlayerId = "" };
    }
}