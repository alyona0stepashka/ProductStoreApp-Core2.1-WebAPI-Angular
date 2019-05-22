using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using App.BLL.Interfaces;
using App.DAL.Interfaces;
using App.Models;

namespace App.BLL.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork _db { get; set; }
        public UserManager<User> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public AdminService(IUnitOfWork uow,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = uow;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return UserManager.Users.ToList();
        }

        public List<IdentityRole> GetAllRoles()
        {
            return RoleManager.Roles.ToList();
        }

        public async Task<User> FindUserById(string id)
        {
            return await UserManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRole(User user)
        {
            return await UserManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddRoleUser(User user, IEnumerable<string> addedRoles)
        {
            return await UserManager.AddToRolesAsync(user, addedRoles);
        }

        public async Task<IdentityResult> RemoveFromRoles(User user, IEnumerable<string> removedRoles)
        {
            return await UserManager.RemoveFromRolesAsync(user, removedRoles);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}