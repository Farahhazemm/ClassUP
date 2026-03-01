using ClassUP.ApplicationCore.DTOs.Requests.Account_Management;
using ClassUP.ApplicationCore.DTOs.Responses.User;
using ClassUP.ApplicationCore.Exceptions;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Services.IAccount;
using ClassUP.ApplicationCore.Services.IImage;
using ClassUP.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassUP.ApplicationCore.Services.Account_Management
{
    public class AccountManagementService : IAccountManagementService
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountManagementService> _logger;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IImageValidator _imageValidator;
        public AccountManagementService(UserManager<AppUser> userManager, ILogger<AccountManagementService> logger, IResetPasswordService resetPasswordService, IImageValidator imageValidator , ICloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _logger=logger;
            _resetPasswordService=resetPasswordService;
            _imageValidator=imageValidator;
            _cloudinaryService=cloudinaryService;
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
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

            await _userManager.UpdateAsync(user);


        }
        #endregion

        #region ChangePassword
        public async Task ChangePasswordAsync(string userId, ChangePasswordDTO dto)
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


        #endregion

        public async Task UpdateProfileImageAsync(string userId, IFormFile profile)
        {
            _imageValidator.Validate(profile);
            var newImage = await _cloudinaryService.UploadProfileImageAsync(profile , userId);

            var user = await _userManager.FindByIdAsync(userId);

            var oldPublicId = user!.ProfileImagePublicId;
            user.ProfilePictureUrl = newImage.Url;
            user.ProfileImagePublicId = newImage.PublicId;

            await _userManager.UpdateAsync(user);
          
            // delete AFTER successful DB update
            if (!string.IsNullOrWhiteSpace(oldPublicId))
                await _cloudinaryService.DeleteAsync(oldPublicId);

        }




    }
}
