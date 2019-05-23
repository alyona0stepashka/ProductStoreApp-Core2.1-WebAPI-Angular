using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces; 
using App.BLL.ViewModels;

namespace App.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //POST: /api/account/Register
        [HttpPost]
        [Route("register")]
        public async Task<object> Register([FromForm]UserRegisterVM model)
        {
            //var url = HttpContext.Request.Host.ToString();
            var result = await _accountService.RegisterUserAsync(model/*, url*/);
            if (result == null)
                return BadRequest(new { message = "Error" });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Route("email/confirm")]
        //[HttpGet("{user_id}", "{code}")]
        //[Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IActionResult> ConfirmEmail(string id/*, string code*/)  //user_id
        {
            if (string.IsNullOrWhiteSpace(id)/* || string.IsNullOrWhiteSpace(code)*/)
            {
                ModelState.AddModelError("", "UserId and Code are required");
                return BadRequest(ModelState);
            }
            var user = await _accountService.GetUserAsync(id);
            if (user == null)
            {
                return BadRequest("Error");
            }
            await _accountService.ConfirmEmailAsync(id/*, code*/);
            return Ok();
        }

        //POST : /api/account/Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm]UserLoginVM model)
        {
            var token = await _accountService.LoginUserAsync(model);
            if (token != null)
                return Ok(new { token });
            return BadRequest(new { message = "Username or password is incorrect or not confirm email." });
        }
    }
}