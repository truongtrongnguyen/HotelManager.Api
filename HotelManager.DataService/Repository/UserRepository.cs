using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        public async Task<bool> UpdateUser(UpdateUserDto request)
        {
            try
            {
                var existing = await _dbSet.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if(existing == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(existing.Avata))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Users", existing.Avata);
                    System.IO.File.Delete(path);
                }

                if (request.Avata != null)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users");
                    if (!Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }

                    var fileName = Path.GetRandomFileName() + Path.GetExtension(request.Avata.FileName);
                    var file = Path.Combine(directory, fileName);

                    using (var streamFile = new FileStream(file, FileMode.Create))
                    {
                        await request.Avata.CopyToAsync(streamFile);
                    }
                    existing.Avata = "/Contents/" + "Users/" + fileName;
                }

                existing.DateUpdate = DateTime.Now;
                existing.FullName = request.FullName;
                existing.Sex = request.Sex;
                existing.BirthDay = request.BirthDay;
                existing.Address = request.Address;
                existing.IdentityCard = request.IdentityCard;
                existing.PhoneNumber = request.PhoneNumber;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(UserRepository)} UpdateUser method has a generated an error");
                return false;
            }
        }
    }
}
