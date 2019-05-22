using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using App.DAL.Data;
using App.DAL.Interfaces; 
using App.Models;
using System.Threading.Tasks;

namespace App.DAL.Repositories
{
    public class UserRepository : IRepository<User, string>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<User> CreateAsync(User item)
        {
            User resUser;
            try
            {
                resUser = (await _db.Users.AddAsync(item)).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resUser = null;
            }
            return resUser;
            // _db.Users.Add(item);
        }

        public async Task<IEnumerable<User>> FindAsync(Func<User, bool> predicate)
        {
            IEnumerable<User> Users = await Task.Factory.StartNew(() => _db.Users.Where(predicate).ToList() as IEnumerable<User>);
            return Users;
            //return _db.Users.Where(predicate).ToList();
        }

        //public void Create(User item)
        //{
        //    _db.Users.Add(item);
        //}

        public async Task<User> DeleteAsync(string id)
        {
            User resUser;
            try
            {
                var item = await _db.Users.FindAsync(id);
                resUser = _db.Users.Remove(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resUser = null;
            }
            return resUser;
            //var User = _db.Users.Find(id);
            //if (User != null)
            //    _db.Users.Remove(User);
        }

        public async Task<User> GetAsync(string id)
        {
            User User;
            try
            {
                User = await _db.Users.FindAsync(id);
            }
            catch
            {
                User = null;
            }
            return User;
            //return _db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            IEnumerable<User> Users = await _db.Users.ToListAsync();
            return Users;
            //return _db.Users
            //.Include(x => x.Users)
            //.Include(m => m.OrderUsers);
        }

        public async Task<User> UpdateAsync(User item)
        {
            User resUser;
            try
            {
                resUser = _db.Users.Update(item).Entity;
                await _db.SaveChangesAsync();
            }
            catch
            {
                resUser = null;
            }
            return resUser;
            //_db.Entry(item).State = EntityState.Modified;
        }

        //public IEnumerable<User> Find(Func<User, bool> predicate)
        //{
        //    return _db.Users
        //        .Include(x => x.Users)
        //        .Where(predicate).ToList();
        //}
    }
}
