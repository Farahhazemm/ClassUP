using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Category
{
    public class UpdateCategoryRequestDTO
    {
        [StringLength(100)] 
        public string? Name { get; set; }
        [StringLength(1000)] 
        public string? Description { get; set; }
    }
}
