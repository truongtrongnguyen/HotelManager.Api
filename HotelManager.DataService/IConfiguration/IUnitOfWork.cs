using HotelManager.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IEmployeeRepository Employees { get; }

        Task CompleteAsync();
    }
}
