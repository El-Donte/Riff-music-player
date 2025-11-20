namespace RiffBackend.Core.Models;

public sealed class User
{
    public const string DEFAULT_AVATAR_PATH = "images/defaultAvatar.jpg";

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string AvatarUrl { get; set; } = string.Empty;

    private User(Guid id, string name, string email, string password, string avatarUrl)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = password;
        AvatarUrl = avatarUrl;
    }

    public static User Create(Guid id, string name, string email, string passwordHash, string avatarUrl)
        => new(id, name.Trim(), email.Trim(), passwordHash, avatarUrl);
}

