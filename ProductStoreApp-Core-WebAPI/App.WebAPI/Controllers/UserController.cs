using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if (user==null)
            {
                return NotFound(new { message = "User not found by id." });
            }
            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditUserInformation([FromBody] UserEditOrShowVM editUser)
        {
            if (editUser == null)
                return BadRequest(new { message = "editUser param is null." });

            var user_id = User.Claims.First(x => x.Type == "UserID").Type;
            editUser.Id = user_id;

            var user = await _userService.EditUserAsync(editUser);
            if (user==null)
            {
                return NotFound(new { message = "User not found by id." });
            }
            return Ok(user);
        }
    }
}