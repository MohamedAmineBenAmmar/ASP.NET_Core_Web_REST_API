using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    // This class will allow us to work with the database
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions) // This will pass the options to the base class constructor
        {
            
        }

        // Creation of the tables
        // Linking the tables to our C# models
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}