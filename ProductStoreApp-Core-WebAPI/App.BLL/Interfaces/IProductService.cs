using System.Collections.Generic;
using App.Models;
using App.BLL.ViewModels;
using System.Threading.Tasks; 

namespace App.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductShowVM>> GetProductsAsync();//
        Task<ProductShowVM> GetProductAsync(int id);//
        Task<ProductEditOrCreateVM> CreateProductAsync(ProductEditOrCreateVM createProduct);//
        Task<ProductEditOrCreateVM> EditProductAsync(ProductEditOrCreateVM editProduct);//
        Task DeleteProductAsync(int id);//
        //Task<IEnumerable<ProductEditOrCreateVM>> FindProductWithPhotosAsync(int id);
        void Dispose();
    }
}
