using App.DAL.Data;
using App.DAL.Interfaces;
using App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private ProductRepository _productRepository;
        private OrderRepository _orderRepository;
        private OrderProductRepository _orderProductRepository;
        private UserRepository _userRepository;
        private FileRepository _fileRepository;

        public EfUnitOfWork(DbContextOptions options)
        {
            _db = new ApplicationDbContext(options);
        }

        public IRepository<Product, int> Products => _productRepository ?? (_productRepository = new ProductRepository(_db));

        public IRepository<Order, int> Orders => _orderRepository ?? (_orderRepository = new OrderRepository(_db));

        public IRepository<OrderProduct, int> OrderProducts => _orderProductRepository ?? (_orderProductRepository = new OrderProductRepository(_db));

        public IRepository<User, string> Users => _userRepository ?? (_userRepository = new UserRepository(_db));

        public IFileRepository<FileModel> FileModels => _fileRepository ?? (_fileRepository = new FileRepository(_db));

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            if (disposing)
            {
                _db.Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
