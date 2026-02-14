using ClassUP.ApplicationCore.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IEmailVerification _emailVerification;

        public EmailVerificationController(IEmailVerification emailVerification)
        {
            _emailVerification = emailVerification;
        }
    }
}
