using HomeStorage.API.Interfaces;
using System.Text;

namespace HomeStorage.API.Middleware
{
    public class ExceptionMiddleware(IServiceScopeFactory scopeFactory) : IMiddleware
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(context, ex);
            }
        }

        public async Task ExceptionHandler(HttpContext context, Exception exception)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IEnumerable<IMiddlewareExceptionHandler> exceptionHandlers = scope.ServiceProvider.GetServices<IMiddlewareExceptionHandler>();

            bool handled = false;
            foreach (IMiddlewareExceptionHandler handler in exceptionHandlers)
            {
                handled = await handler.Handle(context, exception);
                if (handled)
                    return;
            }

            string response = exception.ToString();
            if (exception.InnerException is not null)
                response += ", " + exception.InnerException.ToString();

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.Body.WriteAsync(responseBytes);
            await context.Response.CompleteAsync();
        }
    }

    public static class ExceptionMiddlewareBuilder
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
