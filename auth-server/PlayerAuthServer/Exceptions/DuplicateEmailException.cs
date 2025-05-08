namespace PlayerAuthServer.Exceptions
{
    public class DuplicateEmailException(string message = "Email already in use.") : Exception(message) { }
}