namespace HomeStorage.API.Interfaces
{
    public interface IMiddlewareExceptionHandler
    {
        Task<bool> Handle(HttpContext httpContext, Exception ex);
    }
}
