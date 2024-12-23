namespace HomeStorage.Logic.Exceptions
{
    public class NotAuthorizedException(string? message, Exception? innerException = null) : Exception(message, innerException)
    {
    }
}
