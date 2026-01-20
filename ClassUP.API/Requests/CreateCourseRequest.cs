using System.ComponentModel.DataAnnotations;

namespace ClassUP.API.Requests
{
    public class CreateCourseRequest
    {
        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(2000, MinimumLength = 30)]
        public string Description { get; set; } = null!;

        [Required]
        [Range (0,double.MaxValue)]
        public Decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Level { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public IFormFile Thumbnail { get; set; }

    }
}
