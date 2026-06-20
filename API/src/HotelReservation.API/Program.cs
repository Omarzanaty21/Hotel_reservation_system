using HotelReservation.Application.Extensions;
using HotelReservation.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using HotelReservation.API.Middlewares;

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
