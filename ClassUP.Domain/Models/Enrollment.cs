using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public float ProgressPercentage { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation properties
        public AppUser User { get; set; } = null!;
        public List<LectureProgress> LectureProgresses { get; set; } = [];
    }
}
