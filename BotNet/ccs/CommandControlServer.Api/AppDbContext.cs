using CommandControlServer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Bot> Bots { get; set; }
        public DbSet<BotResponse> BotResponses { get; set; }
        public DbSet<BotGroup> BotGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bot>().ToTable("Bots");
            modelBuilder.Entity<BotResponse>().ToTable("BotResponses");
            modelBuilder.Entity<BotGroup>().ToTable("BotGroups");
        }
    }
}