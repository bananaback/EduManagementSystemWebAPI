using Microsoft.AspNetCore.Mvc;
using StudentManagement.API;
using StudentManagement.Infrastructure;
using StudentManagement.Infrastructure.Services.Authenticators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

builder.Services.AddControllers();

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
    options.AddPolicy("StudentManagementPolicy", policy =>
    {
        policy.WithOrigins("https://myfrontend.com") // Replace with frontend origin soon
              .WithMethods("GET", "POST", "PUT", "DELETE") // Only allowed methods
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

//app.UseCors("StudentManagementPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Use controllers
app.MapControllers();  // Automatically maps the controllers for routing

app.Run();
