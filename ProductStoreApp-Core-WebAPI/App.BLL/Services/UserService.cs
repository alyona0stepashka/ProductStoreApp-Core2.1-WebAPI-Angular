﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModels; 
using App.Models;
using App.DAL.Interfaces;
using AutoMapper;

namespace App.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _db { get; set; }
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork uow,
            IAccountService accountService,
            IMapper mapper)
        {
            _db = uow;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<UserEditOrShowVM> EditUserAsync(UserEditOrShowVM editUser)
        {
            var user = await _accountService.GetUserAsync(editUser.Id);

            try
            {
                var db_user = _mapper.Map<User>(editUser);
                await _db.Users.UpdateAsync(db_user);
                await _db.SaveAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<User> EditUserAvatar(EditUserAvatarViewModel editAvatar)
        //{
        //    var user = await _accountService.GetUser(editAvatar.Id);

        //    try
        //    {
        //        byte[] imageData = null;
        //        using (var binaryReader = new BinaryReader(editAvatar.UploadImage.OpenReadStream()))
        //        {
        //            imageData = binaryReader.ReadBytes((int)editAvatar.UploadImage.Length);
        //        }

        //        user.Image = imageData;

        //        await _db.Users.UpdateAsync(user);
        //        await _db.SaveAsync();
        //        return user;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<User> GetUserAsync(string id)
        //{
        //    if (id == null)
        //        throw new ValidationException("Invalid id", "");

        //    return await _db.Users.GetAsync(id);
        //}
    }
}