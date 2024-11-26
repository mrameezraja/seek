using Microsoft.EntityFrameworkCore;
using Seek.Entities;

namespace Seek.Database
{
    public class SeekDbContext : DbContext
    {
        public DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Seek.db");
        }
    }    
}
