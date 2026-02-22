using ClassUP.ApplicationCore.DTOs.Responses.User;
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
        public async Task<UserProfileDTO> GetProfileAsync(string userId)
        {
            var userProfile = await _userManager.Users
                .Where(x => x.Id == userId)
                .ProjectToType<UserProfileDTO>()
                .SingleAsync();
            // i use single as I am sure That user != null By Token 

            userProfile.UserName = userProfile.Email.Split('@')[0];   // before @

            return userProfile;
        }
    }
}
