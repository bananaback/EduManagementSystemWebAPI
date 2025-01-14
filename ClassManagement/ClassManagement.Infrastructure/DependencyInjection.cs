using ClassManagement.Application.BackgroundServices;
using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Common.Interfaces.Services;
using ClassManagement.Application.Features.Classes.Commands.Create;
using ClassManagement.Infrastructure.Persistence;
using ClassManagement.Infrastructure.Persistence.Repositories;
using ClassManagement.Infrastructure.Services.Authentication;
using ClassManagement.Infrastructure.Services.MessageSenders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ClassManagement.Infrastructure
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
                cfg.RegisterServicesFromAssemblyContaining<CreateClassCommandHandler>();
            });

            services.AddHttpClient("AckApiClient", client =>
            {
                if (hostEnvironment.IsDevelopment())
                {
                    client.BaseAddress = new Uri("https://localhost:7202/");
                }
                else
                {
                    client.BaseAddress = new Uri("https://studentservice-clusterip-srv:8080/");
                }
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IMessageSender, HttpMessageSender>();
            services.AddScoped<IInboxMessageRepository, InboxMessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHostedService<InboxReaderService>();

            return services;
        }
    }
}
