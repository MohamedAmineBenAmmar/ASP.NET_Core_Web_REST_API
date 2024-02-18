using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    // This class will allow us to work with the database
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions) // This will pass the options to the base class constructor
        {

        }

        // Creation of the tables
        // Linking the tables to our C# models
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // Configuring the roles to ensure users can login in otherwise we will get
        // an error without configuring the roles
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole{Name = "User", NormalizedName = "USER"}
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // DON'T FORGET TO RUN THE MIGRATIONS AFTER CREATING THE ROLES !!!!!!!!!
        }
    }
}