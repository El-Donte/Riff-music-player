using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Configures;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Data;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TrackEntity> Tracks { get; set; }
    public DbSet<LikedTracksEntity> LikedTracks { get; set; }
    public DbSet<EmailVerificationTokenEntity> EmailVerificationTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = configuration["ConnectionString"]
           ?? throw new InvalidOperationException("Connection string is missing");

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            npgsqlOptions.CommandTimeout(120);
        });
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

