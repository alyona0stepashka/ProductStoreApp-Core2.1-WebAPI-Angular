using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.BLL.Infrastructure;
using App.BLL.Interfaces;
using App.BLL.ViewModel;
using App.Models.Entities;

namespace App.WEBAPI.Controllers
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
        public IActionResult Get()
        {
            var product = _productService.GetProducts();
            return new ObjectResult(product);
        }

        //GET : api/product/1
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public IActionResult Get(int id)
        {
            var product = _productService.FindProductWithPhotos(id);
            if (product == null)
                return NotFound();
            return new ObjectResult(product);
        }

        //POST : /api/product/CreateProduct
        [HttpPost, Route("CreateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromForm] ProductViewModel createProduct)
        {
            if (createProduct == null)
                return BadRequest();

            const int lengthMax = 2097152;
            const string correctType = "image/jpeg";
            foreach (var uploadedFile in createProduct.Photos)
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

            var newProduct = _productService.CreateProduct(createProduct);
            if (newProduct != null)
                await _fileService.AddPhotosInProductAsync(newProduct.Id, createProduct.Photos);
            BadRequest(new { message = "Error creating product" });
            return Ok(createProduct);
        }

        //PUT : /api/product/EditProduct/1
        [HttpPut, Route("EditProduct/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(int id, [FromForm] EditProductViewModel editProduct)
        {
            if (editProduct == null)
                return BadRequest();

            var product = _productService.GetProduct(id);
            if (product == null)
                return NotFound();

            _productService.EditProduct(product, editProduct);
            return Ok(editProduct);
        }

        //DELETE : /api/product/DeleteProduct/1
        [HttpDelete, Route("DeleteProduct/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                return NotFound();
            _productService.DeleteProduct(id);
            return Ok(product);
        }
    }
}