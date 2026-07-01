
using ErrorHandling_AspNetCore.Data;
using ErrorHandling_AspNetCore.Extensions;
using ErrorHandling_AspNetCore.Implementations;
using ErrorHandling_AspNetCore.Interfaces;
using ErrorHandling_AspNetCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling_AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddProblemDetails();
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IPasswordHasher<Driver>, PasswordHasher<Driver>>();
            //builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Customise 401 Unauthorized response
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse(); // Suppress default response

                var problem = new ProblemDetails
                {
                    Type = "https://myapi.com/errors/unauthorized",
                    Title = "Unauthorized",
                    Status = 401,
                    Detail = "Authentication token is missing or invalid.",
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            },

            // Customise 403 Forbidden response
            OnForbidden = async context =>
            {
                var problem = new ProblemDetails
                {
                    Type = "https://myapi.com/errors/forbidden",
                    Title = "Forbidden",
                    Status = 403,
                    Detail = "You do not have permission to access this resource.",
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            }
        };
    });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseExceptionHandler();
            //app.UseStatusCodePages();
            app.AddGlobalErrorHandlingMiddleware();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.Map("/error/{statusCode:int}", (int statusCode, HttpContext ctx) =>
            {
                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode switch
                    {
                        404 => "Resource Not Found",
                        405 => "Method Not Allowed",
                        _ => "Error"
                    },
                    Instance = ctx.Request.Path
                };
                return Results.Problem(problem);
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
