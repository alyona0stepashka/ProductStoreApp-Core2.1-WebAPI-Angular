using System.Threading.Tasks;
using App.Models;
using App.BLL.ViewModel; 

namespace App.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> EditUser(EditUserInformationViewModel editUser);
        Task<User> EditUserAvatar(EditUserAvatarViewModel editAvatar);
        Task<User> GetUserAsync(string id);
    }
}