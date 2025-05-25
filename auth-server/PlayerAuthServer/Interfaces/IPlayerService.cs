using PlayerAuthServer.Models;

namespace PlayerAuthServer.Interfaces
{
    public interface IPlayerService
    {
        /// <summary>
        /// Creates a new player with the provided details and assigns default settings (password hashing, etc.).
        /// </summary>
        /// <param name="newPlayer">The new player data including email, username, and password hash.</param>
        /// <returns>The created player entity with populated fields (excluding default decks, to be added later).</returns>
        /// <remarks>
        /// Default decks will be assigned in a future update.
        /// </remarks>
        /// <exception cref="DuplicateEmailException">Thrown when the email is already registered.</exception>
        /// <exception cref="DuplicateUsernameException">Thrown when the username is already taken.</exception>
        Task<Player> CreatePlayerWithDefaults(NewPlayer newPlayer);
    }
}