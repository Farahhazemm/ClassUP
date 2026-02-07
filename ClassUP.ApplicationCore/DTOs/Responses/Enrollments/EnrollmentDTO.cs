using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Enrollment
{
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime EnrolledAt { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime? CompletedAt { get; set; }
     
    }
}
