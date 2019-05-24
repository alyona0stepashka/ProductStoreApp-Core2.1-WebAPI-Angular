using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.ViewModels
{
    public class OrderHistoryVM
    {
        public int Id { get; set; }
        public List<int> ProductIds { get; set; }
        public List<string> ProductNames { get; set; }
        public List<decimal> ProductPrices { get; set; }
        public DateTime DatePurchase { get; set; }
    }
}
