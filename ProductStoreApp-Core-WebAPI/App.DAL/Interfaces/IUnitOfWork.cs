using App.Models;
using System;
using System.Threading.Tasks;

namespace App.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product,int> Products { get; }
        IRepository<Order, int> Orders { get; }
        IRepository<OrderProduct, int> OrderProducts { get; }
        IRepository<User, string> Users { get; }
        IFileRepository<FileModel> FileModels { get; }
        Task SaveAsync();
    }
}
