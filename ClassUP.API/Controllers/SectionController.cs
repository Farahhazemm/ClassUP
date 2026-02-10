using ClassUP.API.Extensions;
using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.Services.Sections;
using ClassUP.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        #region Create
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPost("courses/{courseId}/sections")]
        public async Task<IActionResult> Create(int courseId, [FromBody] CreateSectionRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            var section = await _sectionService.CreateAsync(courseId, request, userId, isAdmin);

            return CreatedAtAction("GetById", new { id = section.Id }, section);
        }
        #endregion

        #region Update
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpPut("sections/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSectionRequest request)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _sectionService.UpdateAsync(id, request, userId, isAdmin);

            return NoContent();
        }
        #endregion

        #region Delete
        [Authorize(Roles = AppRoles.User + "," + AppRoles.Admin)]
        [HttpDelete("sections/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole(AppRoles.Admin);

            await _sectionService.DeleteAsync(id, userId, isAdmin);

            return NoContent();
        }
        #endregion

        #region GetById
        [HttpGet("sections/{id}", Name = "GetSectionById")]
        public async Task<IActionResult> GetById(int id)
        {
            var section = await _sectionService.GetByIdAsync(id);
            return Ok(section);
        }
        #endregion

        #region GetCourseSections
        [HttpGet("course/{courseId}/sections")]
        public async Task<IActionResult> GetSectionsByCourse(int courseId)
        {
            var sections = await _sectionService.GetCourseSectionsAsync(courseId);
            return Ok(sections);
        }
        #endregion
    }
}
