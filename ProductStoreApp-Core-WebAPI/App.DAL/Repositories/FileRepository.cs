using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.Data;
using App.DAL.Interfaces;
using App.Models; 

namespace App.DAL.Repositories
{
    public class FileRepository : IFileRepository<FileModel>
    {
        private readonly ApplicationDbContext _db;
        public FileRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<FileModel> CreateAsync(FileModel item)
        {
            FileModel resFileModel;
            try
            {
                resFileModel = (await _db.FileModels.AddAsync(item)).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resFileModel = null;
            }
            return resFileModel;
            // _db.FileModels.Add(item);
        }

        public async Task<IEnumerable<FileModel>> FindAsync(Func<FileModel, bool> predicate)
        {
            IEnumerable<FileModel> filemodels = await Task.Factory.StartNew(() => _db.FileModels.Where(predicate).ToList() as IEnumerable<FileModel>);
            return filemodels;
            //return _db.FileModels.Where(predicate).ToList();
        }
    }
}