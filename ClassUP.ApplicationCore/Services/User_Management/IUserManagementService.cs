using ClassUP.ApplicationCore.DTOs.Requests.User;
using ClassUP.ApplicationCore.DTOs.Responses.UsersManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.User_Management
{
    public interface IUserManagementService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetUserAsync(string id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
        Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO dto);
    }
}
