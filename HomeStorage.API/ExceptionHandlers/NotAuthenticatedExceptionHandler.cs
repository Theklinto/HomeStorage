using HomeStorage.API.Interfaces;
using HomeStorage.Logic.Exceptions;
using System.Net;

namespace HomeStorage.API.ExceptionHandlers
{
    public class NotAuthenticatedExceptionHandler : IMiddlewareExceptionHandler
    {
        public async Task<bool> Handle(HttpContext httpContext, Exception ex)
        {
            if (ex is NotAuthenticatedException is false)
                return false;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await httpContext.Response.CompleteAsync();

            return true;
        }
    }
}
