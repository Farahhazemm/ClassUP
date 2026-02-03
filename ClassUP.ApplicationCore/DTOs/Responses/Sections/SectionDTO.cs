using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Sections
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int OrderIndex { get; set; }
    }
}
