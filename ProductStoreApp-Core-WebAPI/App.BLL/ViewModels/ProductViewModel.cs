using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace App.BLL.ViewModel
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "Error, the minimum number of images should be 1")]
        public List<IFormFile> Photos { get; set; }
    }
}