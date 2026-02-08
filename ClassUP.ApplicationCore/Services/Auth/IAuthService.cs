using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterAsync(RegisterDTO dto);
    }
}
