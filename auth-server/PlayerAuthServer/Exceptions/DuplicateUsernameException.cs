namespace PlayerAuthServer.Exceptions
{
    public class DuplicateUsernameException(string message = "Username already in use.") : Exception(message) { }
}
