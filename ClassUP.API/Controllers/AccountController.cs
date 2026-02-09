using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ClassUP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public AccountController(IAuthService authservice , UserManager<AppUser> userManager,IConfiguration configuration)
        {
            _authservice = authservice;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = await _authservice.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            //1: Check Account Exsist
            AppUser user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            bool found = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!found)
                return Unauthorized("Invalid email or password");


            //2: Generate Token 

            //2.1 Generate a List Of Claims
            var UserRoles = await _userManager.GetRolesAsync(user);

            var Claims = new List<Claim>
            {
               //* token Generated Id NOT User Id It repersent currunt token by JIT *

               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (ClaimTypes.Email, user.Email),

            };
            foreach (var item in UserRoles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, item)); // item = rolename
            }

            //2.2  Prepare my secret Key
            string Key = _configuration["JWT :SecritKey"];
            var KeyinBytes = Encoding.UTF8.GetBytes(Key);
            var signinKey = new SymmetricSecurityKey(KeyinBytes);

            //2.3  SigningCredentials  Take SecretKey & Hash Algo
            SigningCredentials signingCred =
                new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            //2.4 Desigin My token

            JwtSecurityToken MyToken = new JwtSecurityToken
                (
                   audience: _configuration["JWT :Audience"],
                   issuer : _configuration["JWT :Issuer"],
                   claims: Claims,
                  expires: DateTime.UtcNow.AddDays(1),
                   signingCredentials: signingCred


                );
            //2.5 Generate Token (return in our response)

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                expiration = DateTime.Now.AddDays(1)
            });
                
            
           
        }
    }
}
