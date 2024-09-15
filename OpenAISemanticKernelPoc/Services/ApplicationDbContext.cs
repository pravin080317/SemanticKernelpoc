using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using OpenAISemanticKernelPoc.Models;
using OpenAISemanticKernelPoc.Services;

namespace OpenAISemanticKernelPoc.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet represents a table in the database
        public DbSet<Student> Students { get; set; }

        public DbSet<export> exports { get; set; }

        public DbSet<product> products { get; set; }

        public DbSet<Variety> Varietys { get; set; }


    }
}

//The DbContext class is a central part of EF Core. It represents a session with the database, allowing you to query and save data.
//ApplicationDbContext inherits from DbContext. The constructor accepts DbContextOptions and passes them to the base class.
//The DbSet<Student> property represents the table in the database that will store Student entities.
