using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Room;
using HotelManager.Entities.Dto.Outgoing.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room> AddRoom(CreateRoom request);
        Task<bool> UpdateRoom(UpdateRoom room);
        Task<IEnumerable<RoomResponse>> GetAlll();
    }
}
