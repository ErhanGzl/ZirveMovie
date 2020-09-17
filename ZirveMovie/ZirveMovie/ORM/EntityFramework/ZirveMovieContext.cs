using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZirveMovie.Models
{

    public class ZirveMovieContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieCommentPoints> MovieCommentPoints { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token.Token> Token { get; set; }
        public ZirveMovieContext(DbContextOptions<ZirveMovieContext> options) : base(options)
        {
        }

        public ZirveMovieContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

    }
}
