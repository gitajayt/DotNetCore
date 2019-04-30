using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanResource.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly HumanResourceContext _context;

        public DatingRepository(HumanResourceContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
