using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using App.BLL.Interfaces;
using App.DAL.Interfaces;
using App.Models;

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

        //public IEnumerable<FileModel> FindProductPhotos(int id)
        //{
        //    return _db.FileModels.Find(x => x.IdProduct == id);
        //}

        public async Task AddPhotosInProductAsync(int idProduct, List<IFormFile> uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                var path = "/Photos/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                var file = new FileModel
                {
                    Name = uploadedFile.FileName,
                    Path = path,
                    IdProduct = idProduct
                };
                await _db.FileModels.CreateAsync(file);
            }
            await _db.SaveAsync();
        }
    }
}