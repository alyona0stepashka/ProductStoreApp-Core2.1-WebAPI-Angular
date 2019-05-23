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
        public DateTime DateAdded { get; set; }
        public List<string> ImagesURL { get; set; }
    }
}
