using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.BLL.ViewModels;
using App.Models; 

namespace App.BLL.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order newOrder);//
        Task<IEnumerable<Order>> FindOrdersAsync(string userId);//
        Task<IEnumerable<Order>> FindOrdersByDateAsync(DateTime fromDateTime, DateTime toDateTime);
        Task<IEnumerable<OrderHistoryVM>> GetHistoryAsync(string userId);//
        Task<byte[]> SaveResultInExcelAsync(DateTime fromDate, DateTime toDate);
    }
}