using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Category;
using ClassUP.ApplicationCore.DTOs.Responses.Categorises;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Categorise
{
    public class CategoryService : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }


        #region GetAll
        public async Task<PaginatedList<CategoryResponseDTO>> GetAllAsync(FilterOptions op)
        {
            var categories = await _unitOfWork.Categorises.GetAllAsync(op); 

            if (categories == null || !categories.Items.Any())
                return new PaginatedList<CategoryResponseDTO>(
                    new List<CategoryResponseDTO>(), 1, 0, op.PageSize);

       
            var categoryDtos = categories.Items.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

       
            return new PaginatedList<CategoryResponseDTO>(
                categoryDtos,
                categories.PageNumber,
                categories.TotalPages * categories.Items.Count, 
                categoryDtos.Count
            );
        }



        #endregion

        #region GetById
        public async Task<CategoryResponseDTO> GetById(int id)
        {
            var category = await _unitOfWork.Categorises.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found");
            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };

        }
        #endregion

        #region Add
        public async Task<CategoryResponseDTO> AddAsync(CategoryDTO category)
        {
            if (category == null)
                throw new KeyNotFoundException("Categuory info Not found");

            var cat = new Category
            {
               
                Name = category.Name,
                Description = category.Description,
            };

            await _unitOfWork.Categorises.AddAsync(cat);
            await _unitOfWork.SaveChangesAsync();
            return new CategoryResponseDTO
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
            };

        }


        #endregion

        #region Update
        public async Task UpdateAsync(int id, UpdateCategoryRequestDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var categoryFromDb = await _unitOfWork.Categorises.GetByIdAsync(id);
            // Partial Update
            categoryFromDb.Name = string.IsNullOrWhiteSpace(category.Name) ? categoryFromDb.Name : category.Name;
            categoryFromDb.Description = string.IsNullOrWhiteSpace(category.Description) ? categoryFromDb.Description : category.Description;

            await _unitOfWork.Categorises.UpdateAsync(categoryFromDb);
            await _unitOfWork.SaveChangesAsync();


        }


        #endregion

        #region Delete
        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categorises.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found");
            }
            await _unitOfWork.Categorises.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();

        } 
        #endregion

    }
}
