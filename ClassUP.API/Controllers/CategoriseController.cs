using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Category;
using ClassUP.ApplicationCore.DTOs.Responses.Categorises;
using ClassUP.ApplicationCore.Services.Categorise;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories([FromQuery] FilterOptions filter)
        {
            var categories = await _categoryService.GetAllAsync(filter);
            return Ok(categories);
        }
        #endregion

        #region GetById
        [AllowAnonymous]
        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            return Ok(category);
        }
        #endregion

        #region AddCategory
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            var result = await _categoryService.AddAsync(category);
            return CreatedAtAction("GetById", new { categoryId = result.Id }, result);
        }
        #endregion

        #region UpdateCategory
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPatch("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequestDTO category)
        {
            await _categoryService.UpdateAsync(categoryId, category);
            return NoContent();
        }
        #endregion

        #region DeleteCategory
        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return NoContent();
        }
        #endregion
    }
}
