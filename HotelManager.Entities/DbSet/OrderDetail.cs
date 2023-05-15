using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.DbSet
{
    public class OrderDetail
    {
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public int? RoomId { get; set; }
        public Order? Order { get; set; }
        public Room? Room { get; set; }
        public string? RoomName { get; set; }
        public decimal? PriceRoom { get; set; }
        public DateTime? DateCreate { get; set; }

    }
}
