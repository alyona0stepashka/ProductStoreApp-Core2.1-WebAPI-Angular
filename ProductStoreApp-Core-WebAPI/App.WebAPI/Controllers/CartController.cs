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
        private readonly ICartService _cartService;

        public CartController(IOrderService service,
            ISessionHelper helper,
            IProductService product,
            IAccountService account,
            IOrderProductService orderProductService,
            IEmailService emailService,
            ICartService cartService,
            IHostingEnvironment appEnvironment)
        {
            _orderService = service;
            _sessionHelper = helper;
            _productService = product;
            _accountService = account;
            _orderProductService = orderProductService;
            _emailService = emailService;
            _cartService = cartService;
            _appEnvironment = appEnvironment;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            var cart = _sessionHelper
                .GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            if (cart != null)
                return Ok(cart);
            return BadRequest(new { message = "Sorry, your shopping cart is empty." });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> AddProduct(int id)
        {
            var cart = await _cartService.AddProduct(HttpContext, id);
            return Ok(cart);
        }

        [HttpGet]
        [Route("purchase")]
        [Authorize]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            var cart_products = _sessionHelper.GetObjectFromJson<List<CartProductShowVM>>(HttpContext.Session, "cart");
            if (cart_products == null)
            {
                return BadRequest(new { message = "Sorry, your shopping cart is empty!" });
            }

            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var order = await _cartService.BuyAll(HttpContext, cart_products, user_id);
            return Ok(order); 
        }

        [HttpDelete("{id}")] 
        [Authorize]
        public IActionResult RemoveProduct(int id)
        {
            var cart = _cartService.RemoveProduct(HttpContext, id);
            return Ok(cart);
        }

    }
}