using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Reviews
{
    public class CourseReviewDTO
    {
        [Required, Range(1, int.MaxValue)]
        public int CourseId { get; set; }
        
        [Required, Range(1, 5)] 
        public int Rating { get; set; }
        [StringLength(1000)] 
        public string? Comment { get; set; }
    }
}
