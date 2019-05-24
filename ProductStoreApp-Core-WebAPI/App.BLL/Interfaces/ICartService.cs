using App.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface ICartService 
    {
        Task<List<CartProductShowVM>> AddProduct(HttpContext context, int id);//
        List<CartProductShowVM> RemoveProduct(HttpContext context, int id);//
        Task<OrderHistoryVM> BuyAll(HttpContext context, List<CartProductShowVM> cart_products, string user_id); //
    }
}
