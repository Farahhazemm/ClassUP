using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace ClassUP.ApplicationCore.DTOs.Requests.Courses
{
    public class CreateCourseRequest
    {
        
        [StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        
        [StringLength(2000, MinimumLength = 30)]
        public string Description { get; set; } = null!;

       
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        
        [StringLength(50)]
        public string Language { get; set; } = null!;

       
        [StringLength(50)]
        public string Level { get; set; } = null!;

        
        public bool IsActive { get; set; }

        
        
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a valid value")]
        public int CategoryId { get; set; }

        
       public IFormFile Thumbnail { get; set; } = null!;
    }
}
