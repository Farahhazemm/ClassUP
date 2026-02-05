using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Reviews
{
    public class CourseReviewResponseDTO
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
