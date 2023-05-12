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
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
            
        }

        public override async Task<IEnumerable<AppUser>> GetAll()
        {
            try
            {
                return await _dbSet.Where(x => x.IsCustomer == true && x.IsEmployee == false)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{typeof(UserRepository)} GetAll Method has generated an error");
                return new List<AppUser>();
            }
        }

        //public override async Task<bool> Add(AppUser user)
        //{
        //    try
        //    {
        //        user.IsCustomer = true;
        //        user.EmailConfirmed = true;
        //        await _dbSet.AddAsync(user);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"{typeof(UserRepository)} Add method has a generated an error");
        //        return false;
        //    }
        //}
    }
}
