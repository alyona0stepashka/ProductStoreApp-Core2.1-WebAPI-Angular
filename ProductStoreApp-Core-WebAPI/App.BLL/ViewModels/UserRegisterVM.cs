using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.BLL.ViewModels
{
    public class UserRegisterVM
    {
        [Required] 
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }

        [Required] 
        public string Email { get; set; }

        [Required] 
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public IFormFile UploadImage { get; set; }

    }
}

/*
            export class UserRegister {
                FirstName: string,
                LastName: string,
                Email: string,
                Password: string,
                Image: any
            } 
            */
