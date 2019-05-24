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
        }

        public async Task<IEnumerable<OrderHistoryVM>> GetHistoryAsync(string user_id)  //if =null adminOrders
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
                var history_item = new OrderHistoryVM(order);
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

        public async Task<byte[]> SaveResultInExcelAsync(DateTime DateFrom, DateTime DateTo)  //???
        {
            var orderByDate = (await FindOrdersByDateAsync(DateFrom, DateTo)).ToList();
            var countOrder = orderByDate.Count;
            var row_index_begin = 1;
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var options = SaveOptions.XlsxDefault;
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            worksheet.Columns[0].SetWidth(250, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[3].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[4].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[6].SetWidth(350, LengthUnit.Pixel);
            worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            worksheet.Cells["A1"].Value = "User";
            worksheet.Cells["B1"].Value = "DatePurchase";
            worksheet.Cells["C1"].Value = "ProductName";
            worksheet.Cells["D1"].Value = "ProductPrice";
            worksheet.Cells["E1"].Value = "ProductDescription";
            worksheet.Cells["F1"].Value = "Amount";

            worksheet.Cells[1, 6].Value = $"Report of orders by date:" +
                                          $" from {DateFrom.Day}.{DateFrom.Month}.{DateFrom.Year}" +
                                          $" to {DateTo.Day}.{DateTo.Month}.{DateTo.Year}";
            worksheet.Cells[2, 6].Value = $"Number of sales for the specified period : {countOrder}";

            foreach (var order in orderByDate)
            {
                var orderTotalPrice = order.OrderProducts.Sum(m => m.Amount * m.Product.Price);
                var productsCount = order.OrderProducts.Count;
                for (var i = 0; i < productsCount; i++)
                {
                    var style = worksheet.Rows[i].Style;
                    style.Font.Weight = ExcelFont.BoldWeight;
                    style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                }

                for (var r = row_index_begin; r <= productsCount; r++)
                {
                    var item = order.OrderProducts[r - 1];
                    worksheet.Cells[r, 0].Value = item.Order.User.UserName;
                    worksheet.Cells[r, 1].Value = item.Order.PurchaseDate;
                    worksheet.Cells[r, 2].Value = item.Product.Name;
                    worksheet.Cells[r, 3].Value = item.Product.Price;
                    worksheet.Cells[r, 4].Value = item.Product.Description;
                    worksheet.Cells[r, 5].Value = item.Amount;
                    row_index_begin++;
                }
                worksheet.Cells[row_index_begin - 1, 6].Value = $"OrderTotalPrice : {orderTotalPrice} $";
            }
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