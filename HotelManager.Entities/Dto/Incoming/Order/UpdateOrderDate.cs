using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Order
{
    public class UpdateOrderDate
    {
        public int? Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
