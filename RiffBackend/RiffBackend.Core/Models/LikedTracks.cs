namespace RiffBackend.Core.Models;

public sealed class LikedTracks
{
    public Track Track { get; set; }

    public User User { get; set; }

    public DateTime? CreatedAt { get; set; }  = DateTime.Now;
}

