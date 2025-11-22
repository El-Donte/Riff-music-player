namespace RiffBackend.Core.Models;

public sealed class Track
{
    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Author { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }

    public string TrackPath { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public User? User { get; private set; }

    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;

    private Track(Guid id, string title, string author, Guid userId, string trackPath, string imagePath, User? user, DateTime? createdAt)
    {
        Id = id;
        Title = title;
        TrackPath = trackPath;
        ImagePath = imagePath;
        Author = author;
        UserId = userId;
        User = user;
        CreatedAt = createdAt;
    }

    private Track(Guid id, string title, string author, Guid userId)
    {
        Id = id;
        Title = title;
        TrackPath = "";
        ImagePath = "";
        Author = author;
        UserId = userId;
    }

    private Track(Guid id, string title, string author,Guid userId, string trackPath, string imagePath)
    {
        Id = id;
        Title = title;
        TrackPath = trackPath;
        ImagePath = imagePath;
        Author = author;
        UserId = userId;
    }

    public static Track Create(
        Guid id,
        string title,
        string author,
        Guid userId,
        string trackPath,
        string imagePath,
        User? user,
        DateTime? createdAt
        ) => new(id, title.Trim(), author.Trim(), userId, trackPath, imagePath, user, createdAt);

    public static Track Create(
        Guid id,
        string title,
        string author,
        Guid userId,
        string trackPath,
        string imagePath
    ) => new(id, title.Trim(), author.Trim(), userId, trackPath, imagePath);

    public static Track Create(
        Guid id,
        string title,
        string author,
        Guid userId
        ) => new(id, title.Trim(), author.Trim(), userId);
}