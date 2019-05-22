using System; 
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options; 
using App.BLL.Interfaces;
using App.BLL.ViewModel;
using App.DAL.Interfaces; 
using App.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace App.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _db { get; set; }
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationSettings _applicationSettingsOption;

        public AccountService(IUnitOfWork uow,
            UserManager<User> userManager,
            SignInManager<User> signManager,
            IOptions<ApplicationSettings> applicationSettingsOption)
        {
            _db = uow;
            _userManager = userManager;
            _signInManager = signManager;
            _applicationSettingsOption = applicationSettingsOption.Value;
        }

        public async Task<object> RegisterUser(RegisterViewModel model)
        {
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                DateOfRegisters = DateTime.Now
            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "user");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> LoginUser(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _userManager.GetRolesAsync(user);
                var options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id),
                        new Claim(options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault()), 
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettingsOption.JwT_Secret)),
                            SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            else
                return null;
        }

        public async Task<User> GetUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
