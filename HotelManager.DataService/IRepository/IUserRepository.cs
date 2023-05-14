using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<bool> UpdateUser(UpdateUserDto request);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> GetEmployeeByEmail(string email);
    }
}
