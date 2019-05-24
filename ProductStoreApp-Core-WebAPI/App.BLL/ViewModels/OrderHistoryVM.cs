using App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.ViewModels
{
    public class OrderHistoryVM
    {
        public int Id { get; set; }
        //public List<int> ProductIds { get; set; }
        //public List<string> ProductNames { get; set; }
        //public List<decimal> ProductPrices { get; set; }
        public List<CartProductShowVM> Products { get; set; }
        public DateTime DatePurchase { get; set; }

        public OrderHistoryVM(Order order)
        {
            Id = order.Id;
            Products = new List<CartProductShowVM>();
            foreach (var prod in order.OrderProducts)
            {
                Products.Add(new CartProductShowVM(prod));
            }
            DatePurchase = order.PurchaseDate;
        }
    }
}
