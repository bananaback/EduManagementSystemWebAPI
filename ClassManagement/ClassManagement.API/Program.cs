using ClassManagement.API;
using ClassManagement.Application;
using ClassManagement.Infrastructure;
using ClassManagement.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.GetSection("Authentication").Bind(authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

// Register Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Register Controllers
builder.Services.AddControllers();  // This adds API controllers to the DI container

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClassManagementPolicy", policy =>
    {
        policy.WithOrigins("https://myfrontend.com") // Replace with frontend origin soon
              .WithMethods("GET", "POST", "PUT", "DELETE") // Only allowed methods
              .AllowAnyHeader() // Allow custom headers like Authorization
              .AllowCredentials(); // For token-based authentication
    });
});

//app.UseCors("ClassManagementPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Use controllers
app.MapControllers();  // Automatically maps the controllers for routing



app.Run();
