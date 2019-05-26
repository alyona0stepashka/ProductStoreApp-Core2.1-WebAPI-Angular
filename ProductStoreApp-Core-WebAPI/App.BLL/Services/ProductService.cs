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
        //private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public ProductService(IUnitOfWork uow, /*IMapper mapper, */IFileService fileService)
        {
            _db = uow;
            //_mapper = mapper;
            _fileService = fileService;
        }

        public async Task<IEnumerable<ProductShowVM>> GetAllProductsAsync()
        {
            var db_products = (await _db.Products.GetAllAsync()).ToList();
            //var products = _mapper.Map<List<ProductShowVM>>(db_products);
            var products = new List<ProductShowVM>();
            foreach(var db_product in db_products)
            {
                products.Add(new ProductShowVM(db_product));
            }
            return products;
        }

        public async Task<ProductShowVM> GetProductAsync(int id)
        {
            var db_product = await _db.Products.GetAsync(id);
            if (db_product==null)
            {
                return null;
            }
            var product = new ProductShowVM(db_product);
            return product;
        }
        public async Task<Product> GetDbProductAsync(int id)
        {
            var db_product = await _db.Products.GetAsync(id); 
            return db_product;
        }

        public async Task<ProductEditOrCreateVM> CreateProductAsync(ProductEditOrCreateVM createProduct)
        {
            try
            {
                var product = new Product { Name = createProduct.Name, DateAdded = DateTime.Now, Description = createProduct.Description, Price = createProduct.Price };
                var db_prod = await _db.Products.CreateAsync(product);
                if (createProduct.UploadImages != null)
                {
                    foreach (var image in createProduct.UploadImages)
                    {
                        await _fileService.CreatePhotoAsync(image, db_prod.Id);
                    }
                }
                else
                {
                    await _fileService.CreatePhotoAsync(null, db_prod.Id);
                }
                var prod = new ProductEditOrCreateVM(db_prod);
                return prod;
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
                var db_prod = await GetDbProductAsync((int)editProduct.Id);
                if (db_prod==null)
                {
                    return null;
                }

                db_prod.Name = editProduct.Name;
                db_prod.Description = editProduct.Description;
                db_prod.Price = editProduct.Price; 

                if (editProduct.UploadImages!=null)
                {
                    foreach(var file in editProduct.UploadImages)
                    {
                        await _fileService.CreatePhotoAsync(file, editProduct.Id);
                    }
                }
                await _db.Products.UpdateAsync(db_prod);
                var retProduct = new ProductEditOrCreateVM(db_prod);
                return retProduct;
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

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
