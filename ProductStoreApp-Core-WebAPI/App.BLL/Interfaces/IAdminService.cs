using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;
using App.BLL.ViewModels;
using Microsoft.AspNetCore.Identity; 

namespace App.BLL.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<User> GetAllUsers();
        List<IdentityRole> GetAllRoles();
        Task<User> FindUserById(string id);
        Task<IList<string>> GetUserRole(User user);
        Task<IdentityResult> AddRoleUser(User user, IEnumerable<string> addedRoles);
        Task<IdentityResult> RemoveFromRoles(User user, IEnumerable<string> removedRoles);
        void Dispose();
    }
}