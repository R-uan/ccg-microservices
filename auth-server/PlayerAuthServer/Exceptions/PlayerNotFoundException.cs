namespace PlayerAuthServer.Utilities.Exceptions
{
    public class PlayerNotFoundException(string message = "Player was not found.") : Exception(message) { }
}
