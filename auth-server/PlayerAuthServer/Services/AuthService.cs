using PlayerAuthServer.Models;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Exceptions;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Utilities.Exceptions;

namespace PlayerAuthServer.Services
{
    public class AuthService(
        IJwtService jwtService,
        IPlayerService playerService,
        IPlayerRepository playerRepository) : IAuthService
    {
        public async Task<string> AuthenticatePlayer(LoginRequest credentials)
        {
            var player = await playerRepository.FindPlayerByEmail(credentials.Email)
                ?? throw new PlayerNotFoundException("Invalid credentials.");

            bool passwordValid = Bcrypt.Verify(credentials.Password, player.PasswordHash);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return jwtService.GenerateToken(player);
        }

        public async Task<PartialPlayerProfile> RegisterNewPlayer(RegisterRequest request)
        {
            if (await playerRepository.FindPlayerByEmail(request.Email) is not null)
                throw new DuplicateEmailException("Email already registered");

            if (await playerRepository.FindPlayerByUsername(request.Username) is not null)
                throw new DuplicateUsernameException(request.Username);

            var newPlayer = NewPlayer.Create(request);
            var savedPlayer = await playerService.CreatePlayerWithDefaults(newPlayer);
            return PartialPlayerProfile.Create(savedPlayer);
        }
    }
}
