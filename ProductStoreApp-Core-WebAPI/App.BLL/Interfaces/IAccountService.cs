using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.BLL.Infrastructure;
using App.BLL.ViewModel; 

namespace App.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<object> RegisterUser(RegisterViewModel model);
        Task<object> LoginUser(LoginViewModel model);
        Task<User> GetUser(string userId);
        void Dispose();
    }
}
