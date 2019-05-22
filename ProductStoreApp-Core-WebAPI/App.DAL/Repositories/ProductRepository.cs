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
    public class ProductRepository : IRepository<Product,int>
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<Product> CreateAsync(Product item)
        {
            Product resProduct;
            try
            {
                resProduct = (await _db.Products.AddAsync(item)).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resProduct = null;
            }
            return resProduct;
            // _db.Products.Add(item);
        }

        public async Task<IEnumerable<Product>> FindAsync(Func<Product, bool> predicate)
        {
            IEnumerable<Product> Products = await Task.Factory.StartNew(() => _db.Products.Where(predicate).ToList() as IEnumerable<Product>);
            return Products;
            //return _db.Products.Where(predicate).ToList();
        }

        //public void Create(Product item)
        //{
        //    _db.Products.Add(item);
        //}

        public async Task<Product> DeleteAsync(int id)
        {
            Product resProduct;
            try
            {
                var item = await _db.Products.FindAsync(id);
                resProduct = _db.Products.Remove(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resProduct = null;
            }
            return resProduct;
            //var product = _db.Products.Find(id);
            //if (product != null)
            //    _db.Products.Remove(product);
        }

        public async Task<Product> GetAsync(int id)
        {
            Product Product;
            try
            {
                Product = await _db.Products.FindAsync(id);
            }
            catch
            {
                Product = null;
            }
            return Product;
            //return _db.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            IEnumerable<Product> products = await _db.Products.ToListAsync();
            return products;
            //return _db.Products
            //.Include(x => x.Products)
            //.Include(m => m.OrderProducts);
        }

        public async Task<Product> UpdateAsync(Product item)
        {
            Product resProduct;
            try
            {
                resProduct = _db.Products.Update(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resProduct = null;
            }
            return resProduct;
            //_db.Entry(item).State = EntityState.Modified;
        }

        //public IEnumerable<Product> Find(Func<Product, bool> predicate)
        //{
        //    return _db.Products
        //        .Include(x => x.Products)
        //        .Where(predicate).ToList();
        //}
    }
}
