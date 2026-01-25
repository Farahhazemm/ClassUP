using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Cources
{
    public class CreateCourseDTO
    {
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;


        [StringLength(2000, MinimumLength = 30)]
        public string Description { get; set; } = null!;


        // 0 for free courses 
        [Range(0, double.MaxValue)]
        public Decimal Price { get; set; }


        [StringLength(50)]
        public string Language { get; set; } = null!;

        [StringLength(50)]
        // must match => CourseLevel enum 
        public string Level { get; set; }


        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
