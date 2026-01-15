namespace RiffBackend.Core.Models;

public sealed class User
{
    public const string DEFAULT_AVATAR_PATH = "images/defaultAvatar.jpg";

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string AvatarPath { get; set; } = string.Empty;
    
    public bool EmailVerified { get; set; } = false;

    private User(Guid id, string name, string email, string password, string avatarPath, bool emailVerified)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = password;
        AvatarPath = avatarPath;
        EmailVerified = emailVerified;
    }

    public static User Create(Guid id, string name, string email, string passwordHash, string avatarPath, bool emailVerified)
        => new(id, name.Trim(), email.Trim(), passwordHash, avatarPath, emailVerified);
}

