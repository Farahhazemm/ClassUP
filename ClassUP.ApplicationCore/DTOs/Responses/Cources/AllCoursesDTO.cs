using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Cources
{
    public class AllCoursesDTO
    {
        public int Id { get; set; }                     
        public string Title { get; set; } = null!;      
       // public string Description { get; set; } = null!; 
        public decimal Price { get; set; }             
        public CourseLevel Level { get; set; }         
        public string Language { get; set; } = null!;  
        public bool IsActive { get; set; }             
        public string InstructorId { get; set; }          
        public string ThumbnailUrl { get; set; } = null!;
        public int CategoryId { get; set; }


    }
}
