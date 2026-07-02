using ErrorHandling_AspNetCore.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ErrorHandling_AspNetCore.ErrorHandling
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext ctx, Exception ex, CancellationToken ct)
        {
            if (ex is not ValidationException vex) return false;


            var problem = new ValidationProblemDetails(vex.Errors)
            {
                Type = "https://myapi.com/errors/validation",
                Title = "Validation Failed",
                Status =(int) HttpStatusCode.BadRequest,
                Detail = vex.Message,
                Instance = ctx.Request.Path
            };
            problem.Extensions["traceId"] = ctx.TraceIdentifier;

            ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ctx.Response.ContentType = "application/problem+json";

            await ctx.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }
    }

}
