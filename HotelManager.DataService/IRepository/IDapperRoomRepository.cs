using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Room;
using HotelManager.Entities.Dto.Outgoing.Room;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IDapperRoomRepository
    {
        Task<IEnumerable<Room>> GetAll(SqlConnection connect);
        Task<bool> CreateRoom(SqlConnection connection, CreateRoomDapper request);
        Task<bool> DeleteRoomDapper(SqlConnection connection, int Id);
    }
}
