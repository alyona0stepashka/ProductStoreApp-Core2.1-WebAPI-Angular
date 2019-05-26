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
using System.Web;

namespace App.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _db { get; set; }
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationSettings _applicationSettingsOption;
        private readonly IFileService _fileService;
        //private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountService(IUnitOfWork uow,
            UserManager<User> userManager,
            SignInManager<User> signManager,
            IOptions<ApplicationSettings> applicationSettingsOption,
            IEmailService emailService,
            //IMapper mapper,
            IFileService fileService)
        {
            _db = uow;
            _userManager = userManager;
            _signInManager = signManager;
            _applicationSettingsOption = applicationSettingsOption.Value;
            //_mapper = mapper;
            _emailService = emailService;
            _fileService = fileService;
        }

        public async Task<object> RegisterUserAsync(UserRegisterVM model, string url)  
        {
            //var user = _mapper.Map<User>(model);
            var photo_id = await _fileService.CreatePhotoAsync(model.UploadImage, null);
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                FileModelId = photo_id,
                PasswordHash = model.Password,
                UserName = model.Email
            };
            user.DateOfRegisters = DateTime.Now; 
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "user");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encode = HttpUtility.UrlEncode(code);
                var callbackUrl = new StringBuilder("https://")
                    .AppendFormat(url)
                    .AppendFormat("/api/account/email")
                    .AppendFormat($"?user_id={user.Id}&code={encode}");

               // await _emailService.SendEmailAsync(user.Email, "Confirm your account",
               //     $"Confirm the registration by clicking on the link: <a href='{callbackUrl}'>link</a>");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ConfirmEmailAsync(string user_id, string code)
        {
            var db_user = await _userManager.FindByIdAsync(user_id);
            var success = await _userManager.ConfirmEmailAsync(db_user, code);
            //return success.Succeeded ? new OperationDetails(true, "Success", "") : new OperationDetails(false, "Error", "");

            //try
            //{
            //    db_user.EmailConfirmed = true;
            //    await _db.Users.UpdateAsync(db_user); 
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
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
            var db_user = await GetDbUserAsync(user_id);
            if (db_user==null)
            {
                return null;
            }
            //var user = _mapper.Map<UserEditOrShowVM>(db_user);
            var user = new UserEditOrShowVM(db_user);
            return user;
        }

        public async Task<User> GetDbUserAsync(string user_id)
        {
            var db_user = await _userManager.FindByIdAsync(user_id);
            if (db_user == null)
            {
                return null;
            }
            return db_user;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
