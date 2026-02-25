using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db , UserManager<AppUser> userManager) : base(db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<List<AppUser>> GetAllWithRolesAsync()
        {
            
            var users = await _db.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Roles = roles.ToList();
            }

            return users;
        }
    }
}
