using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Section
{
    public class UpdateSectionRequest
    {
        [StringLength(150)] 
        public string? Title { get; set; }
        [Range(1, int.MaxValue)]
        public int? OrderIndex { get; set; }
    }
}
