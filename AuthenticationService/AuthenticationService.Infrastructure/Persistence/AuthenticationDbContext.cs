using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Persistence
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ApplicationUserEntityTypeConfiguration().Configure(modelBuilder.Entity<ApplicationUser>());
            new RefreshTokenEntityTypeConfiguration().Configure(modelBuilder.Entity<RefreshToken>());
        }
    }
}
