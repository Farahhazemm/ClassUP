using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Category
{
    public class UpdateCategoryRequestDTO
    {
        public string? Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }
}
