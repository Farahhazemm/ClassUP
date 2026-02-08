using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Course
    {
        public Course()
        {
            Sections = new List<Section>();
            Requirements = new List<CourseRequirement>();
            Objectives = new List<CourseObjective>();
            CourseTags = new List<CourseTag>();
            Reviews = new List<Review>();
            Enrollments = new List<Enrollment>();
            
        }
        #region MY Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }
        public CourseLevel Level { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public string? PreviewVideoUrl { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
        
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        #endregion

        #region Navigation properties
        public AppUser user { get; set; }
        public Category  Category { get; set; }
        public List<Section> Sections { get; set; }
        public List<CourseRequirement> Requirements { get; set; }
        public List<CourseObjective> Objectives { get; set; }
        public List<CourseTag> CourseTags { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Enrollment> Enrollments { get; set; }
       
        public List<Wishlist> CourseWishlists { get; set; } = new();

        #endregion
    }
}
