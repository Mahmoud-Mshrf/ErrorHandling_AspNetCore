using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ErrorHandling_AspNetCore.ErrorHandling
{
    // ErrorHandling/GlobalExceptionHandler.cs
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(
            IWebHostEnvironment env)
        {
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext ctx, Exception ex, CancellationToken ct)
        {

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Title = "An unexpected error occurred.",
                Status = 500,
                Detail = _env.IsDevelopment() ? ex.Message : "Please contact support.",
                Instance = ctx.Request.Path
            };
            problem.Extensions["traceId"] = ctx.TraceIdentifier;

            if (_env.IsDevelopment())
                problem.Extensions["stackTrace"] = ex.StackTrace;

            ctx.Response.StatusCode = 500;
            ctx.Response.ContentType = "application/problem+json";

            await ctx.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }
    }

}
