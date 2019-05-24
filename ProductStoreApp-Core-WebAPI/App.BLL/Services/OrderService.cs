using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using App.DAL.Interfaces;
using App.Models;
using GemBox.Spreadsheet;

namespace App.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderProductService _orderProductService;
        private IUnitOfWork _db { get; set; }
        public OrderService(IUnitOfWork uow,
            IOrderProductService orderProductService)
        {
            _db = uow;
            _orderProductService = orderProductService;
        }

        public async Task AddOrderAsync(Order newOrder)
        {
            await _db.Orders.CreateAsync(newOrder);
            await _db.SaveAsync();
        }

        public async Task<IEnumerable<OrderHistoryVM>> GetHistoryAsync(string user_id)
        {
            var order_history = new List<OrderHistoryVM>();
            var orders = new List<Order>();
            if (user_id == null)
            {
                orders = (await _db.Orders.GetAllAsync()).ToList();
            }
            else
            {
                orders = (await FindOrdersAsync(user_id)).ToList();
            }            
            foreach (var order in orders)
            {
                var history_item = new OrderHistoryVM {Id=order.Id, DatePurchase=order.PurchaseDate, ProductIds=new List<int>(), ProductNames=new List<string>(), ProductPrices=new List<decimal>() };
                foreach (var product in order.OrderProducts)
                {
                    history_item.ProductIds.Add(product.ProductId);
                    history_item.ProductNames.Add(product.Product.Name);
                    history_item.ProductPrices.Add(product.Product.Price);
                }
                order_history.Add(history_item);
            }
            return  order_history;
        }

        public async Task<IEnumerable<Order>> FindOrdersAsync(string userId)
        {
            return await _db.Orders.FindAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Order>> FindOrdersByDateAsync(DateTime fromDateTime, DateTime toDateTime)
        {
            return await _db.Orders.FindAsync(x => x.PurchaseDate >= fromDateTime && x.PurchaseDate <= toDateTime);
        }

        public async Task<byte[]> SaveResultInExcelAsync(DateTime fromDate, DateTime toDate)
        {
            var orderByDate = (await FindOrdersByDateAsync(fromDate, toDate)).ToList();
            var orderProductByDate = (await _orderProductService.FindOrderProductByOrdersAsync(orderByDate)).ToList();
            var countOrder = orderByDate.Count;
            var orderAmount = _orderProductService.GetOrderAmount(orderProductByDate);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var options = SaveOptions.XlsxDefault;
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            for (var i = 0; i < orderProductByDate.Count; i++)
            {
                var style = worksheet.Rows[i].Style;
                style.Font.Weight = ExcelFont.BoldWeight;
                style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            }

            worksheet.Columns[0].SetWidth(250, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[3].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[4].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[6].SetWidth(350, LengthUnit.Pixel);

            worksheet.Cells["A1"].Value = "User name";
            worksheet.Cells["B1"].Value = "Product purchase date";
            worksheet.Cells["C1"].Value = "The product's name";
            worksheet.Cells["D1"].Value = "Product cost";
            worksheet.Cells["E1"].Value = "Product description";
            worksheet.Cells["F1"].Value = "Amount";

            for (var r = 1; r < orderProductByDate.Count; r++)
            {
                var item = orderProductByDate[r - 1];
                worksheet.Cells[r, 0].Value = item.Order.User.UserName;
                worksheet.Cells[r, 1].Value = item.Order.PurchaseDate;
                worksheet.Cells[r, 2].Value = item.Product.Name;
                worksheet.Cells[r, 3].Value = item.Product.Price;
                worksheet.Cells[r, 4].Value = item.Product.Description;
                worksheet.Cells[r, 5].Value = item.Amount;
            }

            worksheet.Cells[1, 6].Value = $"Report of orders by date: from {fromDate.Day}.{fromDate.Month}.{fromDate.Year} to {toDate.Day}.{toDate.Month}.{toDate.Year}";
            worksheet.Cells[2, 6].Value = $"Number of sales for the specified period : {countOrder}";
            worksheet.Cells[3, 6].Value = $"Amount for the specified period : {orderAmount}";

            var file = GetBytes(workbook, options);
            return file;
        }

        private static byte[] GetBytes(ExcelFile file, SaveOptions options)
        {
            using (var stream = new MemoryStream())
            {
                file.Save(stream, options);
                return stream.ToArray();
            }
        }
    }
}