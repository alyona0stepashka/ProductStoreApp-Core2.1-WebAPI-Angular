using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.ViewModels
{
    public class CartProductShowVM
    {
        public int ProductID { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
