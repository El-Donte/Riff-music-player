using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RiffBackend.Infrastructure.Configures;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TrackEntity> Tracks { get; set; }
        public DbSet<LikedTracksEntity> LikedTracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string useConnection = configuration.GetSection("UseConnection").Value ?? "DefailtConnection";
            string? connectionString = configuration.GetConnectionString(useConnection);
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TrackConfiguration());
            modelBuilder.ApplyConfiguration(new LikedTracksConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
