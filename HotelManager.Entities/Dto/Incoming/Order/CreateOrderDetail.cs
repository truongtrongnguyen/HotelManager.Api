using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Order
{
    public class CreateOrderDetail
    {
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
        public decimal? PriceRoom { get; set; }
    }
}
