using Microsoft.AspNetCore.Mvc;
using PlayerAuthServer.Utilities;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Exceptions;
using PlayerAuthServer.Models.Responses;
using PlayerAuthServer.Utilities.Exceptions;
using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                string token = await service.AuthenticatePlayer(login);
                var response = new LoginResponse(token);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex.Message);
                return ex switch
                {
                    PlayerNotFoundException => Unauthorized(),
                    UnauthorizedAccessException => Unauthorized(),
                    _ => StatusCode(500, "An unexpected error has occurred.")
                };
            }
        }

        /// <summary>
        /// Handles player registration requests.
        /// Validates input and delegates to the player service to create a new account.
        /// Returns appropriate error messages for duplicate usernames or emails.
        /// </summary>
        /// <param name="request">The registration data submitted by the client.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPlayer([FromBody] RegisterRequest request)
        {
            try
            {
                var registeredPlayer = await service.RegisterNewPlayer(request);
                var response = new RegisterResponse(registeredPlayer);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return ex switch
                {
                    DuplicateEmailException => BadRequest("Email already in use."),
                    DuplicateUsernameException => BadRequest("Username already in use."),
                    _ => StatusCode(500, "An unexpected error occurred.")
                };
            }
        }
    }
}