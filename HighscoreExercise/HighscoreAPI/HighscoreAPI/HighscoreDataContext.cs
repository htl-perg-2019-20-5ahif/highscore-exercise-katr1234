using HighscoreAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HighscoreAPI
{
    public class HighscoreDataContext : DbContext
    {
        public HighscoreDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mydb.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath};");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        public DbSet<Highscore> HighscoreLists { get; set; }
    }
}
