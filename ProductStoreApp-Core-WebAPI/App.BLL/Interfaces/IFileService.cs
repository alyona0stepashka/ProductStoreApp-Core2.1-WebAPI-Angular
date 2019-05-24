using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Http; 

namespace App.BLL.Interfaces
{
    public interface IFileService
    {
        //Task<IEnumerable<FileModel>> FindProductPhotos(int product_id);
        //Task AddPhotosInProductAsync(int ProductId, List<IFormFile> uploads);
        Task<int> CreatePhotoAsync(IFormFile photo, int? product_id);
    }
}