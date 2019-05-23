using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Infrastructure;
using App.BLL.Interfaces; 
using App.BLL.ViewModels;
using App.DAL.Interfaces;
using App.Models;
using AutoMapper;

namespace App.BLL.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _db { get; set; }
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductShowVM>> GetProductsAsync()
        {
            var db_products = (await _db.Products.GetAllAsync()).ToList(); 
            var products = _mapper.Map<List<ProductShowVM>>(db_products);
            return products;
        }

        public async Task<ProductShowVM> GetProductAsync(int id)
        {
            var db_product = await _db.Products.GetAsync(id);
            var product = _mapper.Map<ProductShowVM>(db_product);
            return product;
        }

        public async Task<ProductEditOrCreateVM> CreateProductAsync(ProductEditOrCreateVM createProduct)
        {
            try
            {
                createProduct.DateAdded = DateTime.Now;
                var product = _mapper.Map<Product>(createProduct); 
                var db_prod = await _db.Products.CreateAsync(product); 
                return _mapper.Map<ProductEditOrCreateVM>(db_prod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductEditOrCreateVM> EditProductAsync(ProductEditOrCreateVM editProduct)
        {
            try
            {
                var product = _mapper.Map<Product>(editProduct);
                var db_prod = await _db.Products.UpdateAsync(product); 
                return _mapper.Map<ProductEditOrCreateVM>(db_prod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            await _db.Products.DeleteAsync(id); 
        }

        //public async Task<IEnumerable<Product>> FindProductWithPhotosAsync(int id)
        //{
        //    return await _db.Products.FindAsync(x => x.Id == id);
        //}

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
