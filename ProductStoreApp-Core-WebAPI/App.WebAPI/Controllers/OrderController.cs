using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModel;
using App.Models;
using System.Threading.Tasks;

namespace App.WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IProductService _productService;
        private readonly IOrderProductService _orderProductService;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _appEnvironment;

        public OrderController(IOrderService service,
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

        //GET : /api/order
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Get()
        {
            var cart = _sessionHelper
                .GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, "cart");
            if (cart != null)
                return new ObjectResult(cart);
            return BadRequest(new { message = "Sorry, your shopping cart is empty." });
        }

        //POST : /api/order/AddProductInCart/id
        [HttpPost, Route("AddProductInCart/{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> PostAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);
            var elementsInCart = _sessionHelper
                .GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, "cart");
            if (elementsInCart != null)
            {
                var cart = _sessionHelper
                    .GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, "cart");
                var index = IsExist(id);
                if (index != -1)
                    cart[index].Amount++;
                cart.Add(new OrderProduct { Product = product, Amount = 1 });
                _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Ok(cart);
            }
            else
            {
                var cart = new List<OrderProduct>
                {
                    new OrderProduct {Product = product, Amount = 1}
                };
                _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Ok(cart);
            }
        }

        //POST : /api/order/BuyAllProductInCart
        [HttpPost, Route("BuyAllProductInCart")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> BuyAsync()
        {
            var getAllProduct =
                _sessionHelper.GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, key: "cart");
            if (getAllProduct == null)
                return BadRequest(new{ message = "Sorry, your shopping cart is empty!" });

            var userId = User.Claims.First(c => c.Type == "UserID").Value;

            var orderProducts = new List<OrderProduct>();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    PurchaseDate = DateTime.Now
                };
                await _orderService.AddOrderAsync(order);

                foreach (var item in getAllProduct)
                {
                    orderProducts.Add(new OrderProduct
                    {
                        ProductId = item.Product.Id,
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

        //POST : /api/order/RemoveProductFromCart/id
        [HttpPost, Route("RemoveProductFromCart/{id}")]
        [Authorize(Roles = "admin, user")]
        public IActionResult Remove(int id)
        {
            var cart = _sessionHelper
                .GetObjectFromJson<List<OrderProduct>>(HttpContext.Session, "cart");
            var index = IsExist(id);
            cart.RemoveAt(index);
            _sessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return Ok(cart);
        }

        //GET : /api/order/CheckOrderHistory
        [HttpGet, Route("CheckOrderHistory")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DetailsAsync()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;

            var orders = await _orderService.FindOrdersAsync(userId);
            var orderProducts = await  _orderProductService.FindOrderProductByOrdersAsync(orders);

            var orderAmount = _orderProductService.GetOrderAmount(orderProducts);

            var history = new UserOrderHistoryViewModel()
            {
                OrderProduct = orderProducts,
                OrderAmount = orderAmount
            };

            return Ok(history);
        }

        //Get : /api/order/ReportOrderByDate
        [HttpGet, Route("ReportOrderByDate")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReportAsync([FromForm] ReportOrderViewModel model)
        {
            var orderByDate = (await _orderService.FindOrdersByDateAsync(model.FromDate, model.ToDate)).ToList();
            var orderProductByDate = await _orderProductService.FindOrderProductByOrdersAsync(orderByDate);
            var productByDate = orderProductByDate.ToList();

            var countOrder = orderByDate.Count;
            var orderAmount = _orderProductService.GetOrderAmount(productByDate);

            var result = new ResultReportOrderViewModel()
            {
                OrderProduct = productByDate,
                CountOrder = countOrder,
                OrderAmount = orderAmount
            };

            return Ok(result);
        }

        /*
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult OnPostExport(DateTime fromDate, DateTime toDate)
        {
            var options = SaveOptions.XlsxDefault;
            var file = _orderService.SaveResultInExcel(fromDate, toDate);

            return File(file, options.ContentType, "Create." + "XLSX");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult SendExcelFileOnEmailAddress()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SendExcelFileOnEmailAddress(SendExcelFileOnEmailAddress view)
        {
            const int lengthMax = 4194304;
            const string correctType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var type = view.ExcelFile.ContentType;
            var length = view.ExcelFile.Length;

            if (type != correctType)
                ModelState.AddModelError("ExcelFile", "Error, allowed excel-file");
            if (length >= lengthMax)
                ModelState.AddModelError("ExcelFile", "Error, permissible file size should not exceed 4 MB");
            if (!ModelState.IsValid)
                return View(view);

            await _emailService.SendEmail(view.Email, view.ExcelFile);
            return RedirectToAction("Index", "Admin");
        }*/

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