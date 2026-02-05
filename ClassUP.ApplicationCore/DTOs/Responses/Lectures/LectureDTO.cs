using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Lectures
{
    public class LectureDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int SectionId { get; set; }
        public bool IsFree { get; set; }
    }
}
