using App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.ViewModels
{
    public class CartProductShowVM
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CartProductShowVM()
        {

        }
        public CartProductShowVM(Product product)
        {
            ProductId = product.Id;
            Name = product.Name;
            Price = product.Price;
        }
        public CartProductShowVM(OrderProduct product)
        {
            ProductId = product.Product.Id;
            Name = product.Product.Name;
            Price = product.Product.Price;
            Amount = product.Amount;
        }
        public CartProductShowVM(ProductShowVM product)
        {
            ProductId = product.Id;
            Name = product.Name;
            Price = product.Price;
        }

    }
}
/*
 
    export class CartProductShow
    {
        ProductId: number,
        Name: string,
        Price: number,
        Amount: number
    }

*/
