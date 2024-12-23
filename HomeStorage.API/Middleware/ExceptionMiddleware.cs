using HomeStorage.API.Interfaces;

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

            foreach (IMiddlewareExceptionHandler handler in exceptionHandlers)
            {
                bool handled = await handler.Handle(context, exception);
                if (handled)
                    break;
            }
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
