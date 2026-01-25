using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Cources
{
    public class CourseDetailsDTO
    {
        #region Main Prop
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CourseLevel Level { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string? PreviewVideoUrl { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt{get; set; } 
    #endregion

        #region Instructor Info

        public int InstructorId { get; set; }
        #endregion

        #region  Category Info
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        #endregion

        #region  Statistics

        public int TotalSections { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; } 
        #endregion
    }
}
