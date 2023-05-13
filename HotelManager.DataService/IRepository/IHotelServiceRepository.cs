using HotelManager.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IHotelServiceRepository : IGenericRepository<HotelService>
    {
    }
}
