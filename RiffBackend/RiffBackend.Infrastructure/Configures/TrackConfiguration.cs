using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Configures;

internal sealed class TrackConfiguration : IEntityTypeConfiguration<TrackEntity>
{
    public void Configure(EntityTypeBuilder<TrackEntity> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasOne(t => t.User)
            .WithMany(u => u.Tracks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

