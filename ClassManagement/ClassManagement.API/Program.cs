using ClassManagement.Application;
using ClassManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

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

// Use controllers
app.MapControllers();  // Automatically maps the controllers for routing

app.Run();
