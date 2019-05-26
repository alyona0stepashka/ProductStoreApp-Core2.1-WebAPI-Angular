using App.BLL.Interfaces;
using App.BLL.ViewModels;
using App.DAL.Interfaces;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly IOrderProductService _orderProductService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ISessionHelper _sessionHelper;
        //private readonly IMapper _mapper;
        private IUnitOfWork _db { get; set; }
        public CartService(IUnitOfWork uow,
            IOrderProductService orderProductService,
            IProductService productService,
            ISessionHelper sessionHelper,
            //IMapper mapper,
            IOrderService orderService)
        {
            _db = uow;
            _orderProductService = orderProductService;
            _productService = productService;
            _sessionHelper = sessionHelper;
            _orderService = orderService;
           // _mapper = mapper;
        }

        public async Task<List<CartProductShowVM>> AddProduct(HttpContext context, int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return null;
            }
            var cart_products = _sessionHelper.GetObjectFromJson<List<CartProductShowVM>>(context.Session, "cart");
            if (cart_products != null)
            {
                var index = IsAlreadyExist(id, context);
                if (index != -1)
                {
                    cart_products[index].Amount++;
                }
                else
                {
                    cart_products.Add(new CartProductShowVM(product) { Amount=1});
                    _sessionHelper.SetObjectAsJson(context.Session, "cart", cart_products);
                }
                return cart_products;
            }
            else
            {
                var cart = new List<CartProductShowVM>
                {
                    new CartProductShowVM (product) { Amount=1}
                };
                _sessionHelper.SetObjectAsJson(context.Session, "cart", cart);
                return cart;
            }
        }

        public List<CartProductShowVM> RemoveProduct(HttpContext context, int id)
        {
            var cart_products = _sessionHelper.GetObjectFromJson<List<CartProductShowVM>>(context.Session, "cart");
            var index = IsAlreadyExist(id, context);
            if (index != -1)
            {
            cart_products.RemoveAt(index);
            _sessionHelper.SetObjectAsJson(context.Session, "cart", cart_products);
                
            } 
            return cart_products;
        }

        public async Task<OrderHistoryVM> BuyAll(HttpContext context, List<CartProductShowVM> cart_products, string user_id)
        {
            var orderProducts = new List<OrderProduct>();
            var order = new Order
            {
                UserId = user_id,
                PurchaseDate = DateTime.Now
            };
            await _orderService.AddOrderAsync(order);
            foreach (var item in cart_products)
            {
                orderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Amount = item.Amount
                });
            }
            _sessionHelper.RemoveObjectByKey(context.Session,"cart");
            await _orderProductService.AddOrderProductAsync(orderProducts);
            var retOrder = new OrderHistoryVM(order);
            return retOrder;
        }


        private int IsAlreadyExist(int id, HttpContext context)
        {
            var cart = _sessionHelper.GetObjectFromJson<List<OrderProduct>>(context.Session, "cart");
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
