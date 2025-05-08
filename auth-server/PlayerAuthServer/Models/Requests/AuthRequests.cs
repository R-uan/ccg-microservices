using System.ComponentModel.DataAnnotations;

namespace PlayerAuthServer.Models.Requests
{
    /// <summary>
    /// Represents the required data to register a new player account.
    /// All fields are validated for basic integrity before processing.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The player's email address. Must be a valid email format.
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email required")]
        public required string Email { get; set; }

        /// <summary>
        /// The player's chosen username. Must be between 4 and 16 characters.
        /// </summary>
        [Required(ErrorMessage = "Username required")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 16 characters")]
        public required string Username { get; set; }

        /// <summary>
        /// The player's chosen password. Must be at least 8 characters.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string Password { get; set; }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}