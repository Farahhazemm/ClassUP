using ClassUP.ApplicationCore.DTOs.Requests.User;
using ClassUP.ApplicationCore.DTOs.Responses.User;
using ClassUP.ApplicationCore.DTOs.Responses.UsersManagement;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ClassUP.ApplicationCore.Services.User_Management
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManagementService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
          
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

        public async Task <UserDTO>CreateUserAsync(CreateUserDTO dto)
        {
            var EmailIsExsist = await _userManager.Users.AnyAsync(x => x.Email == dto.Email);
            if(EmailIsExsist)
                throw new ConflictException("Email already exists");
            // no same user in my db => make dto into AppUser Form 

            // validate roles
            var allowedRoles = await _roleManager.Roles .Select(r => r.Name!).ToListAsync();
            var invalidRoles = dto.Roles.Except(allowedRoles).ToList();
            if (invalidRoles.Any())
                throw new ConflictException($"Invalid roles: {string.Join(", ", invalidRoles)}");

            var user = dto.Adapt<AppUser>();
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new IdentityOperationException(result.Errors.Select(e => e.Description));

            //  Assign roles
            var roleResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!roleResult.Succeeded)
                throw new IdentityOperationException(roleResult.Errors.Select(e => e.Description));

            return new UserDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                IsDisabled = user.IsDisable,
                LockoutEnd = user.LockoutEnd?.UtcDateTime,
                Roles = dto.Roles.ToList()
            };
        }
    }
}
