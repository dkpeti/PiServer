using Microsoft.EntityFrameworkCore;
using PiServer.Models;

namespace PiServer.Context
{
    public class PiDbContext : DbContext
    {
        public PiDbContext(DbContextOptions<PiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Szenzor> Szenzorok { get; set; }
    }
}
