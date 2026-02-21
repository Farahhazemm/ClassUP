using ClassUP.ApplicationCore.DTOs.Requests.Account.Email;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Login;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
        Task<UserDTO> RegisterAsync(RegisterDTO dto);

        Task ConfirmEmailAsync(ConfirmEmailDTO request );
    }
}
