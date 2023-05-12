using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger _logger;
        internal DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context,
                                ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(object id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
