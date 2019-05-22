using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModel;
using App.DAL.Interfaces;
using App.Models; 

namespace App.BLL.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _db { get; set; }
        public ProductService(IUnitOfWork uow)
        {
            _db = uow;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _db.Products.GetAllAsync();
        }

        public async Task<Product> GetProductAsync(int? id)
        {
            if (id == null)
                throw new ValidationException("Invalid id", "");

            return await _db.Products.GetAsync(id.Value);
        }

        public async Task<Product> CreateProductAsync(ProductViewModel createProduct)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = createProduct.Name,
                    Price = createProduct.Price,
                    Description = createProduct.Description,
                    DateAdded = DateTime.Now
                };

                await _db.Products.CreateAsync(newProduct);
                await _db.SaveAsync();
                return newProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> EditProductAsync(Product product, EditProductViewModel editProduct)
        {
            try
            {
                product.Name = editProduct.Name;
                product.Price = editProduct.Price;
                product.Description = editProduct.Description;
                product.DateAdded = DateTime.Now;

                await _db.Products.UpdateAsync(product);
                await _db.SaveAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            await _db.Products.DeleteAsync(id);
            await _db.SaveAsync();
        }

        public async Task<IEnumerable<Product>> FindProductWithPhotosAsync(int id)
        {
            return await _db.Products.FindAsync(x => x.Id == id);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
