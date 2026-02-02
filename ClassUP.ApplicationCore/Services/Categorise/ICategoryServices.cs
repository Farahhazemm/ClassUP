using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Category;
using ClassUP.ApplicationCore.DTOs.Responses.Categorises;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Categorise
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllAsync(FilterOptions Op);
        Task<CategoryResponseDTO> GetById(int id); 

        Task<CategoryResponseDTO> AddAsync(CategoryDTO category);

        Task UpdateAsync(int id ,UpdateCategoryRequestDTO category);  

        Task DeleteAsync(int id); 
    }
}
