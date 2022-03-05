namespace PaymentGatewayAPI
{
    public static class LoggingMiddleware
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder appBuilder, ILogger logger)
        {
            return appBuilder.Use(async (context, func) =>
            {
                try
                {
                    await func();
                }
                catch (Exception e)
                {
                    logger
                        .LogError("Unhandled controller exception {Exception}", e);
                    context.Response.StatusCode = 500;
                }
            });
        }

    }
}
