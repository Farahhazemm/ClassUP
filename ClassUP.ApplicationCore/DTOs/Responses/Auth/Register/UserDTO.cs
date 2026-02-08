using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Auth.Register
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        public IList<string> Roles
        {
            get; set;

        } = null!;
    }
}
    