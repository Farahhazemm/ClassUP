using ClassUP.ApplicationCore.DTOs.Requests.ForgotPassword;
using ClassUP.ApplicationCore.DTOs.Responses.ForgotPassword;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.ForgotPassword
{
    public interface IForgotPasswordService
    {
        Task RequestAsync(ForgotPasswordRequestDTO dto);
        Task<VerifyForgotPasswordResponseDTO> VerifyAsync(VerifyForgotPasswordDTO dto);
        Task ResetAsync(ResetPasswordDTO dto);
    }

}
