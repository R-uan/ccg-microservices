using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Models
{
    public class NewPlayer
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }

        public static NewPlayer Create(RegisterRequest request)
            => new NewPlayer
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = Bcrypt.HashPassword(request.Password),
            };
    }
}