using System; 
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options; 
using App.BLL.Interfaces; 
using App.DAL.Interfaces; 
using App.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using App.BLL.ViewModels;
using AutoMapper;

namespace App.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _db { get; set; }
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationSettings _applicationSettingsOption;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork uow,
            UserManager<User> userManager,
            SignInManager<User> signManager,
            IOptions<ApplicationSettings> applicationSettingsOption,
            IMapper mapper)
        {
            _db = uow;
            _userManager = userManager;
            _signInManager = signManager;
            _applicationSettingsOption = applicationSettingsOption.Value;
            _mapper = mapper;
        }

        public async Task<object> RegisterUserAsync(UserRegisterVM model)  
        {
            var user = _mapper.Map<User>(model);
            user.DateOfRegisters = DateTime.Now; 
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

        public async Task ConfirmEmailAsync(string user_id)  
        {
            var db_user = await _userManager.FindByIdAsync(user_id);
            try
            {
                db_user.EmailConfirmed = true;
                await _db.Users.UpdateAsync(db_user); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> LoginUserAsync(UserLoginVM model)
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

        public async Task<UserEditOrShowVM> GetUserAsync(string user_id)
        {
            var db_user = await _userManager.FindByIdAsync(user_id);
            var user = _mapper.Map<UserEditOrShowVM>(db_user);
            return user;
        }
        public async Task<User> GetDbUserAsync(string user_id)
        {
            var db_user = await _userManager.FindByIdAsync(user_id); 
            return db_user;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
