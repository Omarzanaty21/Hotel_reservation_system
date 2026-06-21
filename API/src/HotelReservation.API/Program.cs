using HotelReservation.Application.Extensions;
using HotelReservation.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using HotelReservation.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Common;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure CORS from appsettings
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins", policy =>
    {
        policy
            .WithOrigins(corsOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Converts incoming strings to Enums 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var details = context.ModelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage)
        .ToArray();
           
        var response = new ErrorResponse(
            message: "Validation failed",
            errorCode: "VALIDATION_ERROR",
            details: details
        );

        return new BadRequestObjectResult(response);
    };
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowConfiguredOrigins");

// Serve static files (room photos etc.) from wwwroot
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Services.InitialiseDatabase();

app.Run();
