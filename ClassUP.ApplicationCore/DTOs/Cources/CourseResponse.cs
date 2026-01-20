using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Cources
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }                     
        public string Title { get; set; } = null!;      
        public string Description { get; set; } = null!; 
        public decimal Price { get; set; }             
        public CourseLevel Level { get; set; }         
        public string Language { get; set; } = null!;  
        public bool IsActive { get; set; }             
        public int InstructorId { get; set; }          
        public string ThumbnailUrl { get; set; } = null!; 
          
      
    }
}
