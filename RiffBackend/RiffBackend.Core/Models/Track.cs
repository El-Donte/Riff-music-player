namespace RiffBackend.Core.Models;

public class Track
{
    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string TrackPath { get; private set; } = string.Empty;

    public string ImagePath { get; private set; } = string.Empty;

    public string Author { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }

    public User? User { get; private set; }

    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;

    private Track(Guid id, string title, string trackPath, string imagePath, string author, Guid userId ,User? user, DateTime? createdAt)
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

    public static Track Create(
        Guid id, 
        string title, 
        string trackPath, 
        string imagePath,
        string author, 
        Guid userId,
        User? user,
        DateTime? createdAt
        )
    {
        return new Track(id, title.Trim(), trackPath, imagePath, author.Trim(), userId, null, createdAt);
    }
}

