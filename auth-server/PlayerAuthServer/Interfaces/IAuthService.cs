using PlayerAuthServer.Models;
using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a player using their email and password.
        /// Throws <see cref="PlayerNotFoundException"/> or <see cref="UnauthorizedAccessException"/> on failure.
        /// </summary>
        /// <param name="credentials">The login credentials containing email and password.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        Task<string> AuthenticatePlayer(LoginRequest player);

        /// <summary>
        /// Registers a new player, ensures email and username uniqueness, 
        /// and assigns default cards/deck. Returns basic player info.
        /// </summary>
        /// <param name="newPlayer">Registration data with credentials</param>
        /// <returns>Basic player data (DTO)</returns>
        /// <exception cref="DuplicateEmailException"/>
        /// <exception cref="DuplicateUsernameException"/>
        Task<PartialPlayerProfile> RegisterNewPlayer(RegisterRequest newPlayer);
    }

}