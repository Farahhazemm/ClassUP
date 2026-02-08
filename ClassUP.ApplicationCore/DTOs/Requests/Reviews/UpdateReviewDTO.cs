using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Reviews
{
    public class UpdateReviewDTO
    {
        public int  ReviewId { get; set; }

        public string UserId { get; set; }   // temp

        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
