using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        public AdminRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable> GetUsersWithRoles()
        {
            var userList = await _context.Users
                .OrderBy(x => x.UserName)
                .Select(user => new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = (from userRole in user.UserRoles
                             join role in _context.Roles
                             on userRole.RoleId
                             equals role.Id
                             select role.Name).ToList()
                }).ToListAsync();
            return userList;
        }

        public async Task<IEnumerable<string>> EditRoles(string userName, string[] selectedRoles)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return new List<string> {"Faild to add"};

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return new List<string> {"Faild to remove"};

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable> GetPhotosForModeration()
        {
            var photos = await _context.Photos
                .Include(u => u.User)
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .Select(u => new
                {
                    Id = u.Id,
                    UserName = u.User.UserName,
                    Url = u.Url,
                    IsApproved = u.IsApproved
                }).ToListAsync();

            return photos;
        }

        public async Task<int> ApprovePhoto(int photoId)
        {
            var photo = await _context.Photos
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(p => p.Id == photoId);

            photo.IsApproved = true;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RejectPhoto(Photo photo)
        {
            _context.Photos.Remove(photo);

            return await _context.SaveChangesAsync();
        }
    }
}