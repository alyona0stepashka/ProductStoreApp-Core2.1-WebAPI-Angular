using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using App.Models;

namespace App.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        public ProductController(IProductService productService,
            IFileService fileService)
        {
            _productService = productService;
            _fileService = fileService;
        }

        // GET : api/product
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Get()
        {
            var product = await _productService.GetProductsAsync();
            return Ok(product);
        }

        //GET : api/product/1
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        //POST : /api/product/CreateProduct
        [HttpPost, Route("CreateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromForm] ProductEditOrCreateVM createProduct)
        {
            if (createProduct == null)
                return BadRequest();

            const int lengthMax = 2097152;
            const string correctType = "image/jpeg";
            foreach (var uploadedFile in createProduct.UploadImages)
            {
                var type = uploadedFile.ContentType;
                var length = uploadedFile.Length;
                if (type != correctType)
                {
                    ModelState.AddModelError("Uploads", "Error, allowed image resolution jpg / jpeg");
                    return BadRequest(ModelState);
                }

                if (length < lengthMax) continue;
                ModelState.AddModelError("Uploads", "Error, permissible image size should not exceed 2 MB");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newProduct = await _productService.CreateProductAsync(createProduct);
            if (newProduct != null)
                await _fileService.AddPhotosInProductAsync(newProduct.Id, createProduct.UploadImages);
            BadRequest(new { message = "Error creating product" });
            return Ok(createProduct);
        }

        //PUT : /api/product/EditProduct/1
        [HttpPut, Route("EditProduct/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(int id, [FromForm] ProductEditOrCreateVM editProduct)
        {
            if (editProduct == null)
                return BadRequest();

            var db_product = await _productService.GetProductAsync(id);
            if (db_product == null)
                return NotFound();

            var product = await _productService.EditProductAsync(editProduct);
            return Ok(product);
        }

        //DELETE : /api/product/DeleteProduct/1
        [HttpDelete, Route("DeleteProduct/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
                return NotFound();
            await _productService.DeleteProductAsync(id);
            return Ok(product);
        }
    }
}