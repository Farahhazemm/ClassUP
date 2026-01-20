using System.ComponentModel.DataAnnotations;

namespace ClassUP.API.Requests
{
    public class UpdateCourseRequest
    {
        
        [StringLength(150, MinimumLength = 5)]
        public string? Title { get; set; } 

        
        [StringLength(2000, MinimumLength = 30)]
        public string? Description { get; set; } 

        
        [Range(0, double.MaxValue)]
        public Decimal? Price { get; set; }

        
        [StringLength(50)]
        public string? Language { get; set; } 
        
        [StringLength(50)]
        public string? Level { get; set; } 

        
        public bool? IsActive { get; set; }

        
        public IFormFile? Thumbnail { get; set; }

    }
}
