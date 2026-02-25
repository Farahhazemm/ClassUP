using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.UsersManagement
{
      public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public List<string> Roles { get; set; }
    }
}
