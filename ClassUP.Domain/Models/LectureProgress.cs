using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class LectureProgress
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment enrollment { get; set; }
        public int LectureId { get; set; }
        public Lecture lecture { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchedDuration { get; set; } // in seconds
        public DateTime LastWatchedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
