using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.DTOs.Responses.User;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassUP.ApplicationCore.Services.Account_Management
{
    public class AccountManagementService : IAccountManagementService
    {


        private readonly UserManager<AppUser> _userManager;


        public AccountManagementService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        #region GetProfile
        public async Task<UserProfileDTO> GetProfileAsync(string userId)
        {
            /*
             in this method Ican  make by two ways 
            1 - usermanager by findbyId (AllInfo)
            2- DbContext Way => usermaneger.Users (Just Info I need )
            */
            var userProfile = await _userManager.Users
                .Where(x => x.Id == userId)
                .ProjectToType<UserProfileDTO>()
                .SingleAsync();
            // i use single as I am sure That user != null By Token 

            userProfile.UserName = userProfile.Email.Split('@')[0];   // before @

            return userProfile;
        }
        #endregion

        #region UpdateProfile
        public async Task UpdateProfileAsync(string userId, UpdateProfileDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // Iam sure no null user 

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Bio = dto.Bio ?? user.Bio;
            user.ProfilePictureUrl = dto.ProfilePictureUrl ?? user.ProfilePictureUrl;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

            await _userManager.UpdateAsync(user);


        } 
        #endregion

        public async Task ChangePasswordAsync (string userId , ChangePasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // Iam sure no null user

           var result = await _userManager.ChangePasswordAsync(user!, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new IdentityOperationException(errors);

            }
        }
    }
}
