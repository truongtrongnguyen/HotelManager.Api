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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<RefreshToken> GetByRefreshToken(string refreskToken)
        {
            try
            {
                var token = await _dbSet.Where(x => x.Token == refreskToken)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RefreshTokenRepository)} GetByRefreshToken method has generated an error");
                return null;
            }
        }

        public async Task<bool> MarkRefreskTokenAsUsed(RefreshToken refreshToken)
        {
            try
            {
                refreshToken.IsUsed = true;
                _dbSet.Update(refreshToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RefreshTokenRepository)} GetByRefreshToken method has generated an error");
                return false;
            }
        }
    }
}
