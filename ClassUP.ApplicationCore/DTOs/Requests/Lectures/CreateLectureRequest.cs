using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Lectures
{
    public class CreateLectureRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        [RegularExpression("video|article")]
        public string Type { get; set; } 
        public bool IsFree { get; set; } = false;
        public int SectionId { get; set; }

        //contet based type
        public string? ArticleContent { get; set; }
    }
}
