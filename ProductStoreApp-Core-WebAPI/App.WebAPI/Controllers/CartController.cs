using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.WebAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    { 
        private readonly ISessionHelper _sessionHelper; 
        private readonly ICartService _cartService;

        public CartController( 
            ISessionHelper helper, 
            ICartService cartService )
        { 
            _sessionHelper = helper; 
            _cartService = cartService; 
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