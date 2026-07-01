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
                NotFoundException nfe => (HttpStatusCode.NotFound, "Not Found", nfe.Message,"about:blank"),
                Exceptions.NotImplementedException nie => (HttpStatusCode.NotImplemented,"Not Implemented", nie.Message, "about:blank"),
                Exceptions.KeyNotFoundException nkfe => (HttpStatusCode.NotFound, "Key Not Found", nkfe.Message, "about:blank"),
                UnauthorizedAccessException uae => (HttpStatusCode.Unauthorized, "Unauthorized", uae.Message, "about:blank"),
                BadRequestException bre => (HttpStatusCode.BadRequest, "Bad Request", bre.Message, "about:blank"),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message, "about:blank")
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
