using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Configures;

internal sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationTokenEntity>
{
    public void Configure(EntityTypeBuilder<EmailVerificationTokenEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
    }
}