using System.Collections.Generic;
using App.Models;
using App.BLL.ViewModel;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int? id);
        Task<Product> CreateProductAsync(ProductViewModel createProduct);
        Task<Product> EditProductAsync(Product product, EditProductViewModel editProduct);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> FindProductWithPhotosAsync(int id);
        void Dispose();
    }
}
