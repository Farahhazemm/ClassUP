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
    /// <summary>
    /// Handles category management operations (CRUD + listing).
    /// </summary>
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

        /// <summary>
        /// Retrieves all categories with optional filtering and pagination.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories([FromQuery] FilterOptions filter)
        {
            var categories = await _categoryService.GetAllAsync(filter);
            return Ok(categories);
        }

        #endregion

        #region GetById

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(CategoryResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            return Ok(category);
        }

        #endregion

        #region Add

        /// <summary>
        /// Creates a new category (Admin only).
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            var result = await _categoryService.AddAsync(category);

            return CreatedAtAction(
                nameof(GetById),
                new { categoryId = result.Id },
                result
            );
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing category (Admin only).
        /// </summary>
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

        #region Delete

        /// <summary>
        /// Deletes a category by ID (Admin only).
        /// </summary>
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