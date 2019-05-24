using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.BLL.Infrastructure;
using App.BLL.ViewModels; 

namespace App.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<object> RegisterUserAsync(UserRegisterVM model, string url);//
        Task ConfirmEmailAsync(string user_id, string code);//
        Task<object> LoginUserAsync(UserLoginVM model);//
        Task<UserEditOrShowVM> GetUserAsync(string user_id);//
        Task<User> GetDbUserAsync(string user_id);
        void Dispose();
    }
}
