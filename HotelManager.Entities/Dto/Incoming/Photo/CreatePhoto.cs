using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HotelManager.Entities.Dto.Incoming.Photo
{
    public class CreatePhoto
    {
        public string? PhotoName { get; set; }
        public string? PhotoUrl { get; set; }
        public int? RoomId { get; set; }
        public HotelManager.Entities.DbSet.Room? Room { get; set; }
    }
}
