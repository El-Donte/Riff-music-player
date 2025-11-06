using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Configures
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder
                .HasMany(c => c.Tracks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
        }
    }
}
