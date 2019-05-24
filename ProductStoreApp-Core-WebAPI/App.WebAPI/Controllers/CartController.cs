using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using App.Models;
using System.Threading.Tasks;

namespace App.WebAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IProductService _productService;
        private readonly IOrderProductService _orderProductService;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _appEnvironment;

        public CartController(IOrderService service,
            ISessionHelper helper,
            IProductService product,
            IAccountService account,
            IOrderProductService orderProductService,
            IEmailService emailService,
            IHostingEnvironment appEnvironment)
        {
            _orderService = service;
            _sessionHelper = helper;
            _productService = product;
            _accountService = account;
            _orderProductService = orderProductService;
            _emailService = emailService;
            _appEnvironment = appEnvironment;
        }
        [HttpGet]
        //[Route("cart")]
        [Authorize(Roles = "admin, user")]
        public IActionResult Get()
        {
            var cart = _sessionHelper
                .GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            if (cart != null)
                return Ok(cart);
            return BadRequest(new { message = "Sorry, your shopping cart is empty." });
        }

        [HttpGet("{id}")]
        //[Route("cart")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> PostAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);
            var cart_products = _sessionHelper
                .GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            if (cart_products != null)
            {
                var index = IsExist(id);
                if (index != -1)
                {
                    cart_products[index].Amount++;
                }
                else
                {
                    cart_products.Add(new CartProductShowVM { ProductID = product.Id, Amount = 1, Name = product.Name, Price = product.Price });
                    _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart_products);
                }
                return Ok(cart_products);
            }
            else
            {
                var cart = new List<CartProductShowVM>
                {
                    new CartProductShowVM { ProductID = product.Id, Amount = 1, Name = product.Name, Price = product.Price }
                };
                _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Ok(cart);
            }
        }

        //[HttpPost]
        [HttpGet]
        [Route("purchase")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> BuyAsync()
        {
            var cart_products =
                _sessionHelper.GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            if (cart_products == null)
                return BadRequest(new { message = "Sorry, your shopping cart is empty!" });

            var user_id = User.Claims.First(c => c.Type == "UserID").Value;

            var orderProducts = new List<OrderProduct>();
            try
            {
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
                        ProductId = item.ProductID,
                        OrderId = order.Id,
                        Amount = item.Amount
                    });
                }
                await _orderProductService.AddOrderProductAsync(orderProducts);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }

            return Ok(orderProducts);
        }

        [HttpDelete]
        //[Route("delete")]
        [Authorize(Roles = "admin, user")]
        public IActionResult Remove(int id)
        {
            var cart_products = _sessionHelper.GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            var index = IsExist(id);
            cart_products.RemoveAt(index);
            _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart_products);
            return Ok(cart_products);
        }

        private int IsExist(int id)
        {
            var cart = _sessionHelper.GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, "cart");
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