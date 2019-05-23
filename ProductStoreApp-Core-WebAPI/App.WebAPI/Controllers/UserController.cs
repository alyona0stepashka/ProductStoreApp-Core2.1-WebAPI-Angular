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
    [Route("api/user")]
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

        //GET: /api/user/UserProfile
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _accountService.GetUserAsync(userId);
            return Ok(user);
        }

        //PUT: /api/user/EditUserInformation
        [Authorize(Roles = "admin, user")]
        [HttpPut]
        //[Route("EditUserInformation")]
        public async Task<IActionResult> EditUserInformation([FromForm] UserEditOrShowVM editUser)
        {
            if (editUser == null)
                return BadRequest();

            var userId = User.Claims.First(x => x.Type == "UserId").Type;
            editUser.Id = userId;

            var user = await _userService.EditUserAsync(editUser);
            return Ok(user);
        }

        ////PUT: /api/user/EditUserAvatar
        //[Authorize(Roles = "admin, user")]
        //[HttpPut, Route("EditUserAvatar")]
        //public async Task<IActionResult> EditUserAvatar([FromForm] UserEditOrShowVM editAvatar)
        //{
        //    if (editAvatar == null)
        //        return BadRequest();

        //    var userId = User.Claims.First(x => x.Type == "UserId").Type;
        //    editAvatar.Id = userId;

        //    await _userService.EditUserAvatar(editAvatar);
        //    return Ok(editAvatar);
        //}
    }
}