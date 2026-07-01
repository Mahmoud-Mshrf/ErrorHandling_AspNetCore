
using ErrorHandling_AspNetCore.Data;
using ErrorHandling_AspNetCore.Extensions;
using ErrorHandling_AspNetCore.Implementations;
using ErrorHandling_AspNetCore.Interfaces;
using ErrorHandling_AspNetCore.Models;
using Microsoft.AspNetCore.Identity;
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
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
