using ClassUP.ApplicationCore.DTOs.Requests.Auth.Register;
using ClassUP.ApplicationCore.DTOs.Responses.Auth.Register;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO dto)
        {
            //Create Identity User
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
            //Create user with password
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));

                throw new Exception(errors);
            }

            // assign Role
            var roleResult = await _userManager.AddToRoleAsync(user, "Student");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                throw new Exception(errors);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                Roles = roles
            };

        }
    }
}
