using HomeStorage.API.Interfaces;
using HomeStorage.Logic.Exceptions;

namespace HomeStorage.API.ExceptionHandlers
{
    public class NotFoundExceptionHandler : IMiddlewareExceptionHandler
    {
        public async Task<bool> Handle(HttpContext httpContext, Exception ex)
        {
            if (ex is not NotFoundException)
                return false;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.CompleteAsync();

            return true;
        }
    }
}
