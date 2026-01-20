using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Cources
{
    public class UpdateCourseDTO
    {
       
        [StringLength(150, MinimumLength = 5)]
        public string? Title { get; set; } 

        
        [StringLength(2000, MinimumLength = 30)]
        public string? Description { get; set; } 

       
        // 0 for free courses 
        [Range(0, double.MaxValue)]
        public Decimal? Price { get; set; }

        
        [StringLength(50)]
        public string? Language { get; set; } = null!;
        
        [StringLength(50)]
       
        public CourseLevel? Level { get; set; }

        
        public bool? IsActive { get; set; }
    }
}
