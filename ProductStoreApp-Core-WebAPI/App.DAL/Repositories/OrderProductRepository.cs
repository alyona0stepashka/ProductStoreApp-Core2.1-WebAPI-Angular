using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using App.DAL.Data;
using App.DAL.Interfaces; 
using App.Models;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class OrderProductRepository : IRepository<OrderProduct, int>
    {
        private readonly ApplicationDbContext _db;
        public OrderProductRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<OrderProduct> CreateAsync(OrderProduct item)
        {
            OrderProduct resOrderProduct;
            try
            {
                resOrderProduct = (await _db.OrderProducts.AddAsync(item)).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrderProduct = null;
            }
            return resOrderProduct;
            // _db.OrderProducts.Add(item);
        }

        public async Task<IEnumerable<OrderProduct>> FindAsync(Func<OrderProduct, bool> predicate)
        {
            IEnumerable<OrderProduct> OrderProducts = await Task.Factory.StartNew(() => _db.OrderProducts.Where(predicate).ToList() as IEnumerable<OrderProduct>);
            return OrderProducts;
            //return _db.OrderProducts.Where(predicate).ToList();
        }

        //public void Create(OrderProduct item)
        //{
        //    _db.OrderProducts.Add(item);
        //}

        public async Task<OrderProduct> DeleteAsync(int id)
        {
            OrderProduct resOrderProduct;
            try
            {
                var item = await _db.OrderProducts.FindAsync(id);
                resOrderProduct = _db.OrderProducts.Remove(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrderProduct = null;
            }
            return resOrderProduct;
            //var OrderProduct = _db.OrderProducts.Find(id);
            //if (OrderProduct != null)
            //    _db.OrderProducts.Remove(OrderProduct);
        }

        public async Task<OrderProduct> GetAsync(int id)
        {
            OrderProduct OrderProduct;
            try
            {
                OrderProduct = await _db.OrderProducts.FindAsync(id);
            }
            catch
            {
                OrderProduct = null;
            }
            return OrderProduct;
            //return _db.OrderProducts.FindAsync(id);
        }

        public async Task<IEnumerable<OrderProduct>> GetAllAsync()
        {
            IEnumerable<OrderProduct> OrderProducts = await _db.OrderProducts.ToListAsync();
            return OrderProducts;
            //return _db.OrderProducts
            //.Include(x => x.OrderProducts)
            //.Include(m => m.OrderOrderProducts);
        }

        public async Task<OrderProduct> UpdateAsync(OrderProduct item)
        {
            OrderProduct resOrderProduct;
            try
            {
                resOrderProduct = _db.OrderProducts.Update(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resOrderProduct = null;
            }
            return resOrderProduct;
            //_db.Entry(item).State = EntityState.Modified;
        }

        //public IEnumerable<OrderProduct> Find(Func<OrderProduct, bool> predicate)
        //{
        //    return _db.OrderProducts
        //        .Include(x => x.OrderProducts)
        //        .Where(predicate).ToList();
        //}
    }
}
