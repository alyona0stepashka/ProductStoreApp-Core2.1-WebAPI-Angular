using App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface IOrderProductService
    {
        Task AddOrderProductAsync(List<OrderProduct> orderProduct);
        Task<IEnumerable<OrderProduct>> FindOrderProductByOrdersAsync(IEnumerable<Order> order);
        int GetOrderAmount(IEnumerable<OrderProduct> orderList);
    }
}
