using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces;
using App.BLL.ViewModel;

namespace App.WEBAPI.Controllers
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
        [HttpPost, Route("Register")]
        public async Task<object> Register([FromForm]RegisterViewModel model)
        {
            var url = HttpContext.Request.Host.ToString();
            var result = await _accountService.RegisterUser(model/*, url*/);
            if (result == null)
                return BadRequest(new { message = "Error" });
            return Ok(result);
        }

        //GET: /api/account/ConfirmEmail?userid=value&code=value
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }
            var user = await _accountService.GetUser(userId);
            if (user == null)
            {
                return BadRequest("Error");
            }
            var result = await _accountService.ConfirmEmail(user, code);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Message);
        }

        //POST : /api/account/Login
        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
            var token = await _accountService.LoginUser(model);
            if (token != null)
                return Ok(new { token });
            return BadRequest(new { message = "Username or password is incorrect or not confirm email." });
        }
    }
}