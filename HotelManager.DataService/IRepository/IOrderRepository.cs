using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<bool> AddOrder(CreateOrderRequestDto request);
        Task<bool> UpdatePayment(string CustomerName);
        Task<bool> UpdateOrderDateTime(UpdateOrderDate request);
        Task<bool> UpdateOrder(UpdateOrderCustomer request);
    }
}
