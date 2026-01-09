using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class VideoContent
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public Lecture lecture { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } 
        public string VideoUrl { get; set; } 
        public int Duration { get; set; } // in seconds
        public long FileSize { get; set; }
        public string Quality { get; set; } 
        public string ThumbnailUrl { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
