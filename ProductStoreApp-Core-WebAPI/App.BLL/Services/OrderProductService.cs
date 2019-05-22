using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.DAL.Interfaces;
using App.Models;

namespace App.BLL.Services
{
    public class OrderProductService : IOrderProductService
    {
        private IUnitOfWork _db { get; set; }
        public OrderProductService(IUnitOfWork uow)
        {
            _db = uow;
        }

        public async Task AddOrderProductAsync(List<OrderProduct> orderProduct)
        {
            foreach (var item in orderProduct)
            {
                await _db.OrderProducts.CreateAsync(item);
            }
            await _db.SaveAsync();
        }

        public async Task<IEnumerable<OrderProduct>> FindOrderProductByOrdersAsync(IEnumerable<Order> order)
        {
            var list = new List<OrderProduct>();
            foreach (var itemOrder in order)
            {
                list.AddRange(await _db.OrderProducts.FindAsync(x => x.OrderId == itemOrder.Id));
            }

            return list;
        }

        public int GetOrderAmount(IEnumerable<OrderProduct> orderList)
        {
            return orderList.Sum(item => item.Amount * item.Product.Price);
        }
    }
}