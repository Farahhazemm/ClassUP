using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Cources
{
    public class CreateCourseDTO
    {
        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(2000, MinimumLength = 30)]
        public string Description { get; set; } = null!;

        [Required]
        // 0 for free courses 
        [Range(0, double.MaxValue)]
        public Decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = null!;
        [Required]
        [StringLength(50)]
        // must match => CourseLevel enum 
        public CourseLevel Level { get; set; } 

        [Required]
        public bool IsActive { get; set; }
    }
}
