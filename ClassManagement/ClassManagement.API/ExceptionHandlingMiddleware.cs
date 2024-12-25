
using ClassManagement.API.Responses;
using ClassManagement.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace ClassManagement.API
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string message;

            switch (exception)
            {
                case ClassCreationException classCreationException:
                    message = exception.Message;
                    break;
                case ClassRetrievalException classRetrievalException:
                    message = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case ClassPersistenceException classPersistenceException:
                    message = exception.Message;
                    break;

                case StudentCreationException studentCreationException:
                    message = exception.Message;
                    break;
                case StudentRetrievalException studentRetrievalException:
                    message = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case StudentPersistenceException studentPersistenceException:
                    message = exception.Message;
                    break;

                default:
                    message = $"An unexpected error occured. {exception.Message}";
                    break;
            }

            var errorResponse = new ErrorResponse(new[] { message });
            var result = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
