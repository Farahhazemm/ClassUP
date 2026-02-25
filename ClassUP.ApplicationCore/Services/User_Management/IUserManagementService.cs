using ClassUP.ApplicationCore.DTOs.Responses.UsersManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.User_Management
{
    public interface IUserManagementService
    {
        Task<IEnumerable<UsersDTO>> GetAllAsync();
    }
}
