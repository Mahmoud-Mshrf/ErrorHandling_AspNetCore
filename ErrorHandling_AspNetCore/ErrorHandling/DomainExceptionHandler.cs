using ErrorHandling_AspNetCore.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using System.Text.Json;

namespace ErrorHandling_AspNetCore.ErrorHandling
{
    public class DomainExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DomainException dex)
            {
                return false;
            }

            var (statusCode, title, detail, errorCode) = exception switch
            {
                NotFoundException ex => (HttpStatusCode.NotFound,ex.Title,ex.Message,ex.ErrorCode),
                Exceptions.NotImplementedException nie => (HttpStatusCode.NotImplemented, nie.Title, nie.Message, nie.ErrorCode),
                UnauthorizedException uae => (HttpStatusCode.Unauthorized, uae.Title, uae.Message, uae.ErrorCode),
                _ => (HttpStatusCode.BadRequest,(HttpStatusCode.BadRequest).ToString(),exception.Message,"BAD_REQUEST")
            };

            var problem = new ProblemDetails
            {
                Title= title,
                Detail=detail,
                Instance= httpContext.Request.Path,
                Type= errorCode,
                Status=(int)statusCode
            };
            problem.Extensions["traceId"] = httpContext.TraceIdentifier;
            //problem.Extensions["errorCode"] = dex.ErrorCode;
            var result =JsonSerializer.Serialize(problem);
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsync(result,cancellationToken);
            return true;
        }
    }

}
