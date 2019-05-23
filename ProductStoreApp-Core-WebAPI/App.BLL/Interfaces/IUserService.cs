using System.Threading.Tasks;
using App.Models;
using App.BLL.ViewModels; 

namespace App.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserEditOrShowVM> EditUserAsync(UserEditOrShowVM editUser);
        //Task<User> EditUserAvatar(EditUserAvatarViewModel editAvatar);
        //Task<UserEditOrShowVM> GetUserAsync(string id);
    }
}