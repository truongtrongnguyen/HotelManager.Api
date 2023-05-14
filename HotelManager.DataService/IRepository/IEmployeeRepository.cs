using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Employ;
using HotelManager.Entities.Dto.Incoming.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<bool> UpdateEmployee(UpdateEmployeeDto request);
        Task<Employee> GetEmployeeByEmail(string id);
    }
}
