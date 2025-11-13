using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RiffBackend.Infrastructure.Configures;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Data
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
                .AddJsonFile("appsettings.Development.json")
                .Build();
            string useConnection = configuration.GetSection("UseConnection").Value ?? "DefailtConnection";
            string? connectionString = configuration.GetConnectionString(useConnection);
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackEntity>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("timezone('utc', now())")
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TrackConfiguration());
            modelBuilder.ApplyConfiguration(new LikedTracksConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
