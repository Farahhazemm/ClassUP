using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Category;
using ClassUP.ApplicationCore.DTOs.Responses.Categorises;
using ClassUP.ApplicationCore.Services.Categorise;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriseController : ControllerBase
    {
        private readonly ICategoryServices _categoryService;
        public CategoriseController(ICategoryServices categoryServices)
        {
            _categoryService = categoryServices;

        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] FilterOptions filter)
        {
            var categories = await _categoryService.GetAllAsync(filter);

            return Ok(categories);
        }
        #endregion


        #region GetById
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);

            return Ok(category);
        }
        #endregion

        #region AddCategory
        [HttpPost]
      
        public async Task<IActionResult> AddCategory(CategoryDTO category)
        {
            var result = await _categoryService.AddAsync(category);

            return CreatedAtAction("GetById", new { categoryId = result.Id }, result);
        }
        #endregion


        #region UpdateCategory
        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategoryRequestDTO category)
        {
            await _categoryService.UpdateAsync(categoryId, category);
            return NoContent();
        }
        #endregion

        #region DeleteCategory
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return NoContent();
        } 
        #endregion

    }
}
