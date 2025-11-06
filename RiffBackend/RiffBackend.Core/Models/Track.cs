using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiffBackend.Core.Models
{
    public class Track
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string TrackPath { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public User? User { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
