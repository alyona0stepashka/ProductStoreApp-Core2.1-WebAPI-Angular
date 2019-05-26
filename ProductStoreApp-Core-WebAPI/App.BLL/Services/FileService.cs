using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using App.BLL.Interfaces;
using App.DAL.Interfaces;
using App.Models;
using System.Linq;

namespace App.BLL.Services
{
    public class FileService : IFileService
    {
        private IUnitOfWork _db { get; set; }
        private readonly IHostingEnvironment _appEnvironment;
        public FileService(IUnitOfWork uow,
            IHostingEnvironment appEnvironment)
        {
            _db = uow;
            _appEnvironment = appEnvironment;
        }

        //public Task<IEnumerable<FileModel>> FindProductPhotos(int product_id)
        //{
        //    return _db.FileModels.FindAsync(x => x.ProductId == product_id);
        //}

        public async Task<int> CreatePhotoAsync(IFormFile photo, int? product_id)
        {
            var id = 0;
            if (photo != null)
            {
                string path;
                if (product_id != null)
                {
                    path = "/Images/Products/" + photo.FileName;
                }
                else
                {
                    path = "/Images/Users/" + photo.FileName;
                }
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
                var file = new FileModel
                {
                    Name = photo.FileName,
                    Path = path,
                    ProductId = product_id
                };
                await _db.FileModels.CreateAsync(file);
                id = file.Id;
            }
            else
            {
                if (product_id != null)
                {
                    await _db.FileModels.CreateAsync(new FileModel
                    {
                        Name = "no-image.jpg",
                        Path = "/Images/App/no-image.jpg",
                        ProductId = product_id
                    });
                }
                else
                {
                    await _db.FileModels.CreateAsync(new FileModel
                    {
                        Name = "no-image.jpg",
                        Path = "/Images/App/no-image.jpg"
                    });
                }
                id = (await _db.FileModels.FindAsync(m => m.Name == "no-image.jpg")).LastOrDefault().Id;
            }
            return id;
        }

        //public async Task AddPhotosInProductAsync(int ProductId, List<IFormFile> uploads)
        //{
        //    foreach (var uploadedFile in uploads)
        //    {
        //        var path = "/Images/" + uploadedFile.FileName;
        //        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
        //        {
        //            await uploadedFile.CopyToAsync(fileStream);
        //        }
        //        var file = new FileModel
        //        {
        //            Name = uploadedFile.FileName,
        //            Path = path,
        //            ProductId = ProductId
        //        };
        //        await _db.FileModels.CreateAsync(file);
        //    }
        //    await _db.SaveAsync();
        //}
    }
}