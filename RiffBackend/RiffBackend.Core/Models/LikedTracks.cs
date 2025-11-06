using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiffBackend.Core.Models
{
    public class LikedTracks
    {
        public Track Track { get; set; }

        public User User { get; set; }

        public DateTime? CreatedAt { get; set; }  = DateTime.Now;
    }
}
