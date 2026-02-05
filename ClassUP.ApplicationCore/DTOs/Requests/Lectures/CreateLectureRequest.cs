using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Lectures
{
    public class CreateLectureRequest
    {
        [Required]
    public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public LectureType Type { get; set; }

        public bool IsFree { get; set; }

        [Required]
        public int SectionId { get; set; }

        // Article only
        public string? ArticleContent { get; set; }
    }
}
