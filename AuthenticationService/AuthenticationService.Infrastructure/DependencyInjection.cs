using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repository;
using AuthenticationService.Application.Features.Register;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using AuthenticationService.Infrastructure.Services.PasswordHashers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>();
            });

            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
