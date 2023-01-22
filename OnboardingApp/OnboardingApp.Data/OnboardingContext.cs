using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnboardingApp.Data
{
    public class OnboardingContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public OnboardingContext(DbContextOptions<OnboardingContext> options)
            : base(options)
        {
            
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
