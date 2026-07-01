using ErrorHandling_AspNetCore.Exceptions;
using System.Net;
using System.Text.Json;

namespace ErrorHandling_AspNetCore.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
              await  HandleExceptionAsync(context, ex);
            }
        }

        private async static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //HttpStatusCode statusCode;
            //string stackTrace = string.Empty;
            //string message = "";

            var exceptionType = ex;

            var (statusCode, stackTrace, message) = exceptionType switch
            {
                NotFoundException nfe => (HttpStatusCode.NotFound, nfe.StackTrace, nfe.Message),
                Exceptions.NotImplementedException nie => (HttpStatusCode.NotImplemented, nie.StackTrace, nie.Message),
                Exceptions.KeyNotFoundException nkfe => (HttpStatusCode.NotFound, nkfe.StackTrace, nkfe.Message),
                UnauthorizedAccessException uae => (HttpStatusCode.NotFound, uae.StackTrace, uae.Message),
                BadRequestException bre => (HttpStatusCode.NotFound, bre.StackTrace, bre.Message),
                _ => (HttpStatusCode.InternalServerError, ex.StackTrace, ex.Message)
            };

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
