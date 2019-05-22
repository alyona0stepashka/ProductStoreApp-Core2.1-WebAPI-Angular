using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 

namespace App.BLL.Interfaces
{
    public interface IFileService
    {
        //IEnumerable<FileModel> FindProductPhotos(int id);
        Task AddPhotosInProductAsync(int idProduct, List<IFormFile> uploads);
    }
}