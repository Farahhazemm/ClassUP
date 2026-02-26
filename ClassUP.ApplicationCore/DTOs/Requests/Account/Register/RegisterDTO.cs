using System.ComponentModel.DataAnnotations;

namespace ClassUP.ApplicationCore.DTOs.Requests.Auth.Register
{

    public class RegisterDTO
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$",
            ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]
        public string Password { get; set; } = null!;
    }
}
    
