using ErrorHandling_AspNetCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
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

            var (statusCode, title, detail,type) = exceptionType switch
            {
                NotFoundException nfe => (HttpStatusCode.NotFound, nfe.Title, nfe.Message,nfe.ErrorCode),
                Exceptions.NotImplementedException nie => (HttpStatusCode.NotImplemented,nie.Title, nie.Message, nie.ErrorCode),
                UnauthorizedException uae => (HttpStatusCode.Unauthorized,uae.Title , uae.Message, uae.ErrorCode),
                BadRequestException bre => (HttpStatusCode.BadRequest, bre.Title, bre.Message, bre.ErrorCode),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message, "https://tools.ietf.org/html/rfc9110#section-15.6.1")
            };
            var problem = new ProblemDetails
            {
                Type = type,
                Title = title,
                Status =(int) statusCode,
                Detail = detail,
                Instance = context.Request.Path
            };
            problem.Extensions["traceId"] = context.TraceIdentifier;
            var exceptionResult = JsonSerializer.Serialize(problem);
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
