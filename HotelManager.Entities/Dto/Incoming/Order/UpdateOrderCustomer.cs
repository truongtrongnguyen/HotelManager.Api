using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Order
{
    public class UpdateOrderCustomer
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Payment { get; set; }
        public string? RoomName { get; set; }
        public string? IdentityCard { get; set; }
        public string? Adress { get; set; }
        public string? Sex { get; set; }
        public string? Status { get; set; }
    }
}
