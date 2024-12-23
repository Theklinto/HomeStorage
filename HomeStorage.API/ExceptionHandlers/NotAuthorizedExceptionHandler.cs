using HomeStorage.API.Interfaces;
using HomeStorage.Logic.Exceptions;
using System.Net;

namespace HomeStorage.API.ExceptionHandlers
{
    public class NotAuthorizedExceptionHandler : IMiddlewareExceptionHandler
    {
        public async Task<bool> Handle(HttpContext httpContext, Exception ex)
        {
            if (ex is not NotAuthorizedException)
                return false;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await httpContext.Response.CompleteAsync();


            return true;
        }
    }
}
