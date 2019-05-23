using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Interfaces;
using App.BLL.ViewModel;

namespace App.WEBAPI.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet, Route("UserProfile")]
        public async Task<object> GetUserProfile()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _accountService.GetUser(userId);
            return new
            {
                user.FirstName,
                user.LastName,
                user.DateOfRegisters,
                user.UserName
            };
        }

        //PUT: /api/user/EditUserInformation
        [Authorize(Roles = "admin, user")]
        [HttpPut, Route("EditUserInformation")]
        public async Task<object> EditUserInformation([FromForm] EditUserInformationViewModel editUser)
        {
            if (editUser == null)
                return BadRequest();

            var userId = User.Claims.First(x => x.Type == "UserId").Type;
            editUser.Id = userId;

            await _userService.EditUser(editUser);
            return Ok(editUser);
        }

        //PUT: /api/user/EditUserAvatar
        [Authorize(Roles = "admin, user")]
        [HttpPut, Route("EditUserAvatar")]
        public async Task<object> EditUserAvatar([FromForm] EditUserAvatarViewModel editAvatar)
        {
            if (editAvatar == null)
                return BadRequest();

            var userId = User.Claims.First(x => x.Type == "UserId").Type;
            editAvatar.Id = userId;

            await _userService.EditUserAvatar(editAvatar);
            return Ok(editAvatar);
        }
    }
}