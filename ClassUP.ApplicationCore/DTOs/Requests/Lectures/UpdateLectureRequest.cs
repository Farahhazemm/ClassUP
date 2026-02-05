using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Lectures
{
    public class UpdateLectureRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; } 
        public bool? IsFree { get; set; }
    }
}
