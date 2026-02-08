using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Reviews
{
    public class CourseReviewDTO
    {
        public int CourseId { get; set; }
        public string StudentId { get; set; } // Temp

        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
