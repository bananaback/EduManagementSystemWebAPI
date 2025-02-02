﻿using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Common.Interfaces.TokenGenerators;
using AuthenticationService.Application.Common.Interfaces.TokenValidators;
using AuthenticationService.Application.Features.Register;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using AuthenticationService.Infrastructure.Services.Authenticators;
using AuthenticationService.Infrastructure.Services.PasswordHashers;
using AuthenticationService.Infrastructure.Services.TokenGenerators;
using AuthenticationService.Infrastructure.Services.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Automatically apply migrations if in Production
            if (hostEnvironment.IsProduction())
            {
                using var SP = services.BuildServiceProvider();
                using var scope = SP.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
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
                cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>();
            });

            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddScoped<ITokenValidator, RefreshTokenValidator>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
