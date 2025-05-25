namespace PlayerAuthServer.Models;

public class AuthenticatedPlayer
{
    public required Guid PlayerId { get; set; }
    public required string Username { get; set; }
    public required bool IsBanned { get; set; }
    
    public static AuthenticatedPlayer Create(Player player)
    {
        return new AuthenticatedPlayer
        {
            PlayerId = player.Id,
            Username = player.Username,
            IsBanned = player.IsBanned,
        };
    }
}