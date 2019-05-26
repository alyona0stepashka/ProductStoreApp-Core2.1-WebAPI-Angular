using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfRegisters { get; set; }

        [ForeignKey("FileModel")]
        public int FileModelId { get; set; }

        public virtual FileModel FileModel { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
