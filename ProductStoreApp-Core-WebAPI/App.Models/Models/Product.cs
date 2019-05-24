﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; } 

        public virtual List<FileModel> FileModels { get; set; }
        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}
