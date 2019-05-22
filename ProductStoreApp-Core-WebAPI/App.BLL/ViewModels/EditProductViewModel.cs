using System.ComponentModel.DataAnnotations;

namespace App.BLL.ViewModel
{
    public class EditProductViewModel
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
    }
}