using App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.ViewModels
{
    public class ProductShowVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        //public DateTime DateAdded { get; set; }
        public List<string> ImagesURL { get; set; }

        public ProductShowVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            ImagesURL = new List<string>();
            foreach(var img in product.FileModels)
            {
                ImagesURL.Add(img.Path);
            }
        }
    }
}
