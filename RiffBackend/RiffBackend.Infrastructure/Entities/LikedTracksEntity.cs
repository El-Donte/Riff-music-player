using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiffBackend.Infrastructure.Entities
{
    [Table("liked_tracks")]
    public class LikedTracksEntity
    {
        [Key, Column("user_id",Order = 0)]
        public Guid UserId { get; set; }

        [Key, Column("track_id",Order = 1)]
        public Guid TrackId { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(UserId))]
        public UserEntity? User { get; set; }

        [ForeignKey(nameof(TrackId))]
        public TrackEntity? Track { get; set; }
    }
}
