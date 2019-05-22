using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfRegisters { get; set; }

        public byte[] Image { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
