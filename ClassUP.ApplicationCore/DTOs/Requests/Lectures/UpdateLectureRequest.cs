using ClassUP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Lectures
{
    public class UpdateLectureRequest
    {
        [StringLength(150)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }
        public string? Type { get; set; }
        public bool? IsFree { get; set; }
    }
}
