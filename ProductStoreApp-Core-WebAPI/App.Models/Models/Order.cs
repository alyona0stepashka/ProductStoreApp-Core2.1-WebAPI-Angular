using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public virtual User User { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}
