using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.DTOs.Responses.Sections;
using ClassUP.ApplicationCore.Services.Sections;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        /// <summary>
        /// Create a new section under a specific course.
        /// Only course owner or admin is allowed.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("courses/{courseId}/sections")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(typeof(SectionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(int courseId, [FromBody] CreateSectionRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            var section = await _sectionService.CreateAsync(courseId, request, userId, isAdmin);

            return CreatedAtAction(nameof(GetById), new { id = section.Id }, section);
        }

        /// <summary>
        /// Update an existing section.
        /// Only course owner or admin is allowed.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPut("sections/{id}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSectionRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _sectionService.UpdateAsync(id, request, userId, isAdmin);

            return NoContent();
        }

        /// <summary>
        /// Delete a section by id.
        /// Only course owner or admin is allowed.
        /// </summary>
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("sections/{id}")]
        [EnableRateLimiting("userlimit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _sectionService.DeleteAsync(id, userId, isAdmin);

            return NoContent();
        }

        /// <summary>
        /// Get section by its ID.
        /// </summary>
        [HttpGet("sections/{id}", Name = "GetSectionById")]
        [ProducesResponseType(typeof(SectionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var section = await _sectionService.GetByIdAsync(id);
            return Ok(section);
        }

        /// <summary>
        /// Get all sections for a specific course.
        /// </summary>
        [HttpGet("course/{courseId}/sections")]
        [ProducesResponseType(typeof(IEnumerable<SectionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionsByCourse(int courseId)
        {
            var sections = await _sectionService.GetCourseSectionsAsync(courseId);
            return Ok(sections);
        }
    }
}