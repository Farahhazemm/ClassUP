using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Section
{
    public class UpdateSectionRequest
    {
        public string? Title { get; set; } = null!;
        public int? OrderIndex { get; set; }
    }
}
