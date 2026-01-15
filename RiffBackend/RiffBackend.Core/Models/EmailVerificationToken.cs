namespace RiffBackend.Core.Models;

public sealed class EmailVerificationToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public User User { get; set; }
}