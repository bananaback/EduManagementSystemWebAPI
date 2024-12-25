using AuthenticationService.Application.Exceptions;

namespace AuthenticationService.API
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            var (statusCode, message) = MapExceptionToResponse(exception);

            var errorResponse = new
            {
                Message = message,
                Details = isDevelopment ? exception.Message : null, // Only include details in development
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(errorResponse);
        }

        private static (int statusCode, string message) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                DuplicatedUserException duplicatedUserException => (StatusCodes.Status500InternalServerError, "Register user failed"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.") // Default case
            };
        }


    }

}
