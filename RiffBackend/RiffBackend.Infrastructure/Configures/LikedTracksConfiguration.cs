using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Configures
{
    public class LikedTracksConfiguration : IEntityTypeConfiguration<LikedTracksEntity>
    {
        public void Configure(EntityTypeBuilder<LikedTracksEntity> builder)
        {
            builder.HasKey(lt => new { lt.UserId, lt.TrackId });

            builder.HasOne(lt => lt.User)
                .WithMany(u => u.LikedTracks)
                .HasForeignKey(lt => lt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(lt => lt.Track)
                .WithMany(t => t.LikedByUsers)
                .HasForeignKey(lt => lt.TrackId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
