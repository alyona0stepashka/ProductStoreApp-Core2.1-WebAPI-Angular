using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    { 
        private readonly IOrderService _orderService; 

        public OrderController(IOrderService service )
        {
            _orderService = service; 
        }


        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllHistories()
        {
            var order_history = (await _orderService.GetHistoryAsync(null)).ToList();
            if (order_history==null)
            {
                return NotFound(new { message = "Orders not found." });
            }
            return Ok(order_history);
        }

        [HttpGet]
        [Route("user")]
        [Authorize]
        public async Task<IActionResult> GetUserHistories()
        {
            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var order_history = (await _orderService.GetHistoryAsync(user_id)).ToList();
            if (order_history == null)
            {
                return NotFound(new { message = "Orders not found by user." });
            }
            return Ok(order_history);
        }

        [HttpGet]
        [Route("admin/date")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllHistoriesByDate(DateTime dateFrom, DateTime dateTo)
        {
            var order_history = (await _orderService.GetHistoryAsync(null)).ToList();
            if (order_history == null)
            {
                return NotFound(new { message = "Orders not found by dateRange." });
            }
            var ret_order_history = order_history.Where(m => m.DatePurchase <= dateTo && m.DatePurchase >= dateFrom);
            return Ok(ret_order_history);
        }

        [HttpGet]
        [Route("user/date")]
        [Authorize]
        public async Task<IActionResult> GetUserHistoriesByDate(DateTime dateFrom, DateTime dateTo)
        {
            var user_id = User.Claims.First(c => c.Type == "UserID").Value;
            var order_history = (await _orderService.GetHistoryAsync(user_id)).ToList();
            if (order_history == null)
            {
                return NotFound(new { message = "Orders not found by dateRange." });
            }
            var ret_order_history = order_history.Where(m => m.DatePurchase <= dateTo && m.DatePurchase >= dateFrom);
            return Ok(ret_order_history);
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