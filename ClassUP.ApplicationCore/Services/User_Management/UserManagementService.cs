using ClassUP.ApplicationCore.DTOs.Responses.UsersManagement;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

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
        public async Task<IEnumerable<UsersDTO>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllWithRolesAsync();
           
            var result = await Task.WhenAll(users.Select(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new UsersDTO
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email!,
                    IsDisabled = user.IsDisable,
                    LockoutEnd = user.LockoutEnd?.UtcDateTime,
                    Roles = roles.ToList()
                };
            }));

            return result;
        }
    }
}
