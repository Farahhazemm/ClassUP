using ClassUP.ApplicationCore.Services.Section;
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
    }
}
