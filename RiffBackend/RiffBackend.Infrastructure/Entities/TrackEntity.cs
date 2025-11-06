using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiffBackend.Infrastructure.Entities
{
    [Table("tracks")]
    public class TrackEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("author")]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Column("track_path")]
        public string TrackPath { get; set; } = string.Empty;

        [Column("image_path")]
        public string ImagePath { get; set; } = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Column("user_id")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity? User { get; set; }

        public ICollection<LikedTracksEntity> LikedByUsers { get; set; } = new List<LikedTracksEntity>();
       
    }
}
