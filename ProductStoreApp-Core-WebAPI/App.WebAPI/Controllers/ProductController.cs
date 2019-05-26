using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Interfaces;
using App.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService; 
        public ProductController(IProductService productService )
        {
            _productService = productService; 
        }

        [HttpGet]
        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productService.GetAllProductsAsync();
            return Ok(product);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found by id." });
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductEditOrCreateVM createProduct)
        {
            if (createProduct == null)
                return BadRequest(new { message = "createProduct param is null." });

            if (createProduct.UploadImages != null)
            {
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
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newProduct = await _productService.CreateProductAsync(createProduct);
            return Ok(newProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] ProductEditOrCreateVM editProduct)
        {
            if (editProduct == null)
                return BadRequest(new { message = "editProduct param is null." });

            var db_product = await _productService.GetProductAsync(id);
            if (db_product == null)
                return NotFound(new { message = "Product not found by id." });

            var product = await _productService.EditProductAsync(editProduct);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found by id." });
            await _productService.DeleteProductAsync(id);
            return Ok(product);
        }
    }
}