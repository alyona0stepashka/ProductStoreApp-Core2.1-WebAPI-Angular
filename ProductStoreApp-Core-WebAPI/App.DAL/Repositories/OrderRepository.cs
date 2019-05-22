using System;
using Microsoft.EntityFrameworkCore;
using App.DAL.Data;
using App.DAL.Interfaces; 
using System.Collections.Generic;
using System.Linq;
using App.Models;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class OrderRepository : IRepository<Order, int>
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<Order> CreateAsync(Order item)
        {
            Order resOrder;
            try
            {
                resOrder = (await _db.Orders.AddAsync(item)).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrder = null;
            }
            return resOrder;
            // _db.Orders.Add(item);
        }

        public async Task<IEnumerable<Order>> FindAsync(Func<Order, bool> predicate)
        {
            IEnumerable<Order> Orders = await Task.Factory.StartNew(() => _db.Orders.Where(predicate).ToList() as IEnumerable<Order>);
            return Orders;
            //return _db.Orders.Where(predicate).ToList();
        }

        //public void Create(Order item)
        //{
        //    _db.Orders.Add(item);
        //}

        public async Task<Order> DeleteAsync(int id)
        {
            Order resOrder;
            try
            {
                var item = await _db.Orders.FindAsync(id);
                resOrder = _db.Orders.Remove(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrder = null;
            }
            return resOrder;
            //var Order = _db.Orders.Find(id);
            //if (Order != null)
            //    _db.Orders.Remove(Order);
        }

        public async Task<Order> GetAsync(int id)
        {
            Order Order;
            try
            {
                Order = await _db.Orders.FindAsync(id);
            }
            catch
            {
                Order = null;
            }
            return Order;
            //return _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            IEnumerable<Order> Orders = await _db.Orders.ToListAsync();
            return Orders;
            //return _db.Orders
            //.Include(x => x.Orders)
            //.Include(m => m.OrderOrders);
        }

        public async Task<Order> UpdateAsync(Order item)
        {
            Order resOrder;
            try
            {
                resOrder = _db.Orders.Update(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrder = null;
            }
            return resOrder;
            //_db.Entry(item).State = EntityState.Modified;
        }

        //public IEnumerable<Order> Find(Func<Order, bool> predicate)
        //{
        //    return _db.Orders
        //        .Include(x => x.Orders)
        //        .Where(predicate).ToList();
        //}
    }
}
