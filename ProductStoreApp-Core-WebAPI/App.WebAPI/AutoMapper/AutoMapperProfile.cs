using App.BLL.ViewModels;
using App.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.WebAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        //UserViewModel viewModel = _mapper.Map<UserViewModel>(user);
        public AutoMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<Product, ProductEditOrCreateVM>().ReverseMap();
            CreateMap<Product, ProductShowVM>().ReverseMap();
            CreateMap<User, UserRegisterVM>().ReverseMap();
            CreateMap<User, UserEditOrShowVM>().ReverseMap();
            CreateMap<User, UserLoginVM>().ReverseMap();
        }
    }
}
