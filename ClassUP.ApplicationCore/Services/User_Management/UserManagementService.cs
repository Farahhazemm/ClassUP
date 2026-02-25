using ClassUP.ApplicationCore.DTOs.Responses.User;
using ClassUP.ApplicationCore.DTOs.Responses.UsersManagement;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ClassUP.ApplicationCore.Services.User_Management
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public UserManagementService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllWithRolesAsync();
           
            var result = await Task.WhenAll(users.Select(async user =>
            {
                
                return new UserDTO
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email!,
                    IsDisabled = user.IsDisable,
                    LockoutEnd = user.LockoutEnd?.UtcDateTime,
                    Roles = user.Roles
                };
            }));

            return result;
        }

        public async Task<UserDTO> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new NotFoundException("User", id);

            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                IsDisabled = user.IsDisable,
                LockoutEnd = user.LockoutEnd?.UtcDateTime,
                Roles = userRoles.ToList()
            };
        }
    }
}
