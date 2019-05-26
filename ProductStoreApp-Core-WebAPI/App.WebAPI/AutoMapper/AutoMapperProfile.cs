using App.BLL.ViewModels;
using App.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.WebAPI.AutoMapper
{
    //public class AutoMapperProfile : Profile
    //{
    //    //UserViewModel viewModel = _mapper.Map<UserViewModel>(user);
    //    public AutoMapperProfile()
    //    {
    //        AllowNullCollections = true;

    //        CreateMap<Product, ProductEditOrCreateVM>().ReverseMap();
    //           // .ForMember("Login", opt => opt.MapFrom(src => src.)));
            
            
    //        ;
    //        CreateMap<Product, ProductShowVM>()
    //          //  .ForMember("", m=>m.MapFrom(e=>e.)) ImagesURL
    //            .ReverseMap()
    //           // .ForMember("", m => m.MapFrom(e=>e.)
    //            ;
    //        CreateMap<User, UserRegisterVM>().ReverseMap();
    //        CreateMap<User, UserEditOrShowVM>().ReverseMap();
    //        CreateMap<User, UserLoginVM>().ReverseMap();
    //        CreateMap<Order, OrderHistoryVM>().ReverseMap();
    //    }
    //}
}
