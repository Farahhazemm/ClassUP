namespace ClassUP.ApplicationCore.DTOs.Responses.User
{
    public class UserProfileDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }
    }
}