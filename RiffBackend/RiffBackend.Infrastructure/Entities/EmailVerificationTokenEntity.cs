using System.ComponentModel.DataAnnotations.Schema;

namespace RiffBackend.Infrastructure.Entities;

[Table("emailVerificationTokens")]
public sealed class EmailVerificationTokenEntity
{
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public UserEntity? User { get; set; }
}