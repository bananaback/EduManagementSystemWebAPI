using Microsoft.AspNetCore.Mvc;
using Serilog;
using StudentManagement.API;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Services.Authenticators;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.GetSection("Authentication").Bind(authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);


// Register Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddCors(options =>
{
    options.AddPolicy("StudentManagementPolicy", policy =>
    {
        policy.WithOrigins("https://myfrontend.com") // Replace with frontend origin soon
              .WithMethods("GET", "POST", "PUT", "DELETE") // Only allowed methods
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers();

// Register Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;  // This adds API controllers to the DI container

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseRouting();

//app.UseCors("StudentManagementPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Use controllers
app.MapControllers();  // Automatically maps the controllers for routing

app.Run();
