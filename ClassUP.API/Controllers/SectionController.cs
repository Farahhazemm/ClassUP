using ClassUP.ApplicationCore.DTOs.Requests.Section;
using ClassUP.ApplicationCore.Services.Sections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// Endpoints for Sections
// POST /api/courses/{courseId}/sections
// GET /api/sections/{id}
// PUT /api/sections/{id}
// DELETE /api/sections/{id}
// GET /api/courses/{courseId}/sections
namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService )
        {
            _sectionService = sectionService;
        }

        #region Create
        [HttpPost("courses/{courseId}/sections")]
        public async Task<IActionResult> Create(int courseId, [FromBody] CreateSectionRequest request)
        {
            var section = await _sectionService.CreateAsync(courseId, request);
            return NoContent();
            // return CreatedAtAction("GetById", new { sectionId = section.Id }, section);
        }
        #endregion

        #region Update
        [HttpPut("sections/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSectionRequest request)
        {
            await _sectionService.UpdateAsync(id, request);
            return NoContent();
        } 
        #endregion

    }
}
