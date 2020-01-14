using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAdminRepository
    {
        Task<IEnumerable> GetUsersWithRoles();
        Task<IEnumerable<string>> EditRoles(string userName, string[] selectedRoles);
        Task<IEnumerable> GetPhotosForModeration();
        Task<int> ApprovePhoto(int photoId);
        Task<int> RejectPhoto(Photo photo);
    }
}