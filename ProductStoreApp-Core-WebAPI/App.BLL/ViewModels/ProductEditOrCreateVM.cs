using App.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.BLL.ViewModels
{
    public class ProductEditOrCreateVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string  Description{ get; set; }

        [Required]
        public decimal Price { get; set; }

        //public DateTime DateAdded { get; set; }

        public List<IFormFile> UploadImages { get; set; }  //for edit = null 

        public ProductEditOrCreateVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
        }
    }
}
