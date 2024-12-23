namespace HomeStorage.Logic.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException(string message) : base(message) { }
        public NotAuthenticatedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
