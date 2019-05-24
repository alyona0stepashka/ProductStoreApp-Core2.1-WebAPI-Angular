using App.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.BLL.ViewModels
{
    public class UserEditOrShowVM
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string Email { get; set; } //for edit = null
        public string ImageURL { get; set; } //for edit = null 
        public IFormFile UploadImage { get; set; }

        public UserEditOrShowVM(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            ImageURL = user.FileModel.Path;
        }
    }
}
