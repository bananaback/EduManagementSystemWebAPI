using ClassManagement.Domain.Entities;
using ClassManagement.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ClassEntityTypeConfiguration().Configure(modelBuilder.Entity<Class>());
            new StudentEntityTypeConfiguration().Configure(modelBuilder.Entity<Student>());
            new EnrollmentEntityTypeConfiguration().Configure(modelBuilder.Entity<Enrollment>());   
            new InboxMessageEntityTypeConfiguration().Configure(modelBuilder.Entity<InboxMessage>());   
        }
    }
}
