using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces;
using App.BLL.ViewModels;

namespace App.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public UserController(IAccountService accountService,
            IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }
         
        [HttpGet] 
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _accountService.GetUserAsync(user_id);
            return Ok(user);
        }
         
        [HttpPut]
        [Authorize] 
        public async Task<IActionResult> EditUserInformation([FromForm] UserEditOrShowVM editUser)
        {
            if (editUser == null)
                return BadRequest();

            var user_id = User.Claims.First(x => x.Type == "UserId").Type;
            editUser.Id = user_id;

            var user = await _userService.EditUserAsync(editUser);
            return Ok(user);
        } 
    }
}