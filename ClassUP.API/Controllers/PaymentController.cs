using ClassUP.ApplicationCore.Services.Cart;
using ClassUP.ApplicationCore.Services.Enrollment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IEnrollmentService _enrollmentService;
        

        public PaymentController(ICartService cartService , IEnrollmentService enrollmentService)
        {
            _cartService = cartService;
            _enrollmentService = enrollmentService; 
        }
    }
}
