using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Repository
{
    public class HotelServiceRepository : GenericRepository<HotelService>, IHotelServiceRepository
    {
        public HotelServiceRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
            
        }

        public override async Task<IEnumerable<HotelService>> GetAll()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(HotelServiceRepository)} GetAll Method has generated an error");
                return new List<HotelService>();
            }
        }

        public override async Task<bool> Update(HotelService request)
        {
            try
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (existing == null)
                {
                    return false;
                }

                existing.Title = request.Title;
                existing.Description = request.Description;
                existing.Price = request.Price;
                existing.Quantity = request.Quantity;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(HotelServiceRepository)} GetAll Method has generated an error");
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (existing == null)
                {
                    return false;
                }

                _dbSet.Remove(existing);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(HotelServiceRepository)} Delete Method has generated an error");
                return false;
            }
        }
    }
}
