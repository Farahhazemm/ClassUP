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
            var usersWithRoles = await (
                from u in _db.Users
                join ur in _db.UserRoles on u.Id equals ur.UserId into userRoles
                from ur in userRoles.DefaultIfEmpty()
                join r in _db.Roles on ur.RoleId equals r.Id into roles
                from r in roles.DefaultIfEmpty()
                group r by new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.IsDisable,
                    u.LockoutEnd
                } into g
                select new AppUser
                {
                    Id = g.Key.Id,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    Email = g.Key.Email,
                    IsDisable = g.Key.IsDisable,
                    LockoutEnd = g.Key.LockoutEnd,
                    Roles = g.Where(x => x != null).Select(x => x.Name).ToList()!
                }
            ).ToListAsync();

            return usersWithRoles;
        }
    }
}
