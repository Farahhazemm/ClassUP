using ClassUP.ApplicationCore.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Account_Management
{
    public interface IAccountManagementService
    {
        Task<UserProfileDTO> GetProfileAsync(string userId);
    }
}
