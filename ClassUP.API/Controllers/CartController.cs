using ClassUP.ApplicationCore.Services.Cart;
using ClassUP.ApplicationCore.Services.Courses;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICourseService _courseServices;
        public CartController(ICartService cartService , ICourseService courseService)
        {
            _courseServices = courseService;
            _cartService = cartService;
        }
    }
}
