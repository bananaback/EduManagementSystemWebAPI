using AuthenticationService.API;
using AuthenticationService.Infrastructure;
using AuthenticationService.Infrastructure.Services.TokenGenerators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.GetSection("Authentication").Bind(authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

builder.Services.AddInfrastructure(builder.Configuration);





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
    options.AddPolicy("AuthenticationPolicy", policy =>
    {
        policy.WithOrigins("https://myfrontend.com") // Replace with frontend origin soon
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow cookies or Authorization headers
    });
});

//app.UseCors("AuthenticationPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();