using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Application.BackgroundServices;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Commons.Interfaces.Services;
using StudentManagement.Application.Features.Students.Commands.Create;
using StudentManagement.Infrastructure.Persistence;
using StudentManagement.Infrastructure.Persistence.Repositories;
using StudentManagement.Infrastructure.Services.Authenticators;
using StudentManagement.Infrastructure.Services.MessageSenders;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Automatically apply migrations if in Production
            if (hostEnvironment.IsProduction())
            {
                using var SP = services.BuildServiceProvider();
                using var scope = SP.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            var serviceProvider = services.BuildServiceProvider();
            var authenticationConfiguration = serviceProvider.GetRequiredService<AuthenticationConfiguration>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudiences = authenticationConfiguration.Audiences,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateStudentCommandHandler>();
            });

            services.AddHttpClient("InboxApiClient", client =>
            {
                if (hostEnvironment.EnvironmentName == Environments.Development)
                {
                    client.BaseAddress = new Uri("https://localhost:7233/");
                }
                else
                {
                    client.BaseAddress = new Uri("http://classservice-clusterip-srv:8080/");
                };
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageSender, HttpMessageSender>();
            services.AddHostedService<OutboxReaderService>();


            return services;
        }
    }
}
