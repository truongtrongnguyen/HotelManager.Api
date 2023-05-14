using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Employ;
using HotelManager.Entities.Dto.Incoming.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
            
        }

        public async Task<Employee> GetEmployeeByEmail(string id)
        {
            try
            {
                var existing = await _dbSet.Where(x => x.IdentityId == id)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

                if (existing != null)
                {
                    return existing;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(EmployeeRepository)} Method GetEmployeeByEmail has generated an error");
                return null;
            }
        }


        public async Task<bool> UpdateEmployee(UpdateEmployeeDto request)
        {
            try
            {
                var existing = await _dbSet.Where(x => x.IdentityId == request.Id).FirstOrDefaultAsync();

                if (existing == null)
                {
                    return false;
                }

                existing.Position = request.Position;
                existing.Allowvance = request.Allowvance;
                existing.BankNumber = request.BankNumber;
                existing.Salary = request.Salary;
                existing.DateContact = request.DateContact;
                existing.ContactTerm = request.ContactTerm;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(EmployeeRepository)} UpdateEmployee method has a generated an error");
                return false;
            }
        }

    }
}
