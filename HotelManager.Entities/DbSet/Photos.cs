using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelManager.Entities.DbSet
{
    public class Photos
    {
        public int Id { get; set; }
        public string? PhotoName { get; set; }
        public string? PhotoUrl { get; set; }
        public int? RoomId { get; set; }
        [JsonIgnore]
        public Room? Room { get; set; }
        

    }
}
