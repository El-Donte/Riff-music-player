using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiffBackend.Infrastructure.Entities
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        [Required]
        public string Password { get; set; } = string.Empty;

        [Column("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        public ICollection<TrackEntity> Tracks { get; set; } = new List<TrackEntity>();
        public ICollection<LikedTracksEntity> LikedTracks { get; set; } = new List<LikedTracksEntity>();
    }
}
