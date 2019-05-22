using Microsoft.AspNetCore.Http;

namespace App.BLL.ViewModel
{
    public class EditUserAvatarViewModel
    {
        public string Id { get; set; }
        public IFormFile UploadImage { get; set; }
    }
}