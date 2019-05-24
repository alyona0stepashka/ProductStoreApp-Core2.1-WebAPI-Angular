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
    [Route("api/order")]
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
         
         
        [HttpGet]
        [Route("history/admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DetailsAsync()
        { 
            var order_history = (await _orderService.GetHistoryAsync(null)).ToList(); 
            return Ok(order_history);
        }

        [HttpGet]
        [Route("history/user")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DetailsUserAsync()
        {
            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var order_history = (await _orderService.GetHistoryAsync(user_id)).ToList();
            return Ok(order_history);
        }

        [HttpGet]
        [Route("history/date/admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DetailsDateAsync(DateTime dateFrom, DateTime dateTo)
        {
            var order_history = (await _orderService.GetHistoryAsync(null)).ToList().Where(m=>m.DatePurchase<=dateTo && m.DatePurchase>=dateFrom);
            return Ok(order_history);
        }

        [HttpGet]
        [Route("history/date/user")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DetailsDateUserAsync(DateTime dateFrom, DateTime dateTo)
        {
            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var order_history = (await _orderService.GetHistoryAsync(user_id)).ToList().Where(m => m.DatePurchase <= dateTo && m.DatePurchase >= dateFrom);
            return Ok(order_history);
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

    }
}