using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.DbSet
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Payment { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string? CustomerId { get; set; }
        public string? RoomName { get; set; }
        public string? IdentityCard { get; set; }
        public string? Adress { get; set; }
        public string? Sex { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? Status { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
