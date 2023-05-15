using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context,
                                ILogger logger)
                                : base(context, logger)
        {
            
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                return await _dbSet.OrderByDescending(x => x.DateCreate)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(OrderRepository)} GetAll Method has generated an error");
                return new List<Order>();
            }
        }

        public async Task<bool> UpdatePayment(string CustomerName)
        {
            try
            {
                var order = _dbSet.Include(x => x.OrderDetails).Where(x => x.CustomerName == CustomerName).FirstOrDefault();

                if(order == null)
                {
                    return false;
                }

                order.Status = "Đã trả";
                order.Payment = "Đã thanh toán";
                
                foreach (var item in order.OrderDetails)
                {
                    var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == item.RoomId);
                    if (room == null)
                    {
                        return false;
                    }
                    room.Status = "Còn trống";
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(OrderRepository)} UpdatePayment has generated an error");
                return false;
            }
        }

        public async Task<bool> UpdateOrderDateTime(UpdateOrderDate request)
        {
            try
            {
                var order = await _dbSet.Include(x =>x.OrderDetails).FirstOrDefaultAsync(x => x.Id == request.Id);

                if (order == null)
                {
                    return false;
                }

                order.CheckIn = request.CheckIn;
                order.CheckOut = request.CheckOut;

                decimal totalAmount = 0;

                if (order.OrderDetails?.Count > 0)
                {
                    foreach (var item in order.OrderDetails)
                    {
                        var existRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == item.RoomId);

                        if (existRoom == null)
                        {
                            return false;
                        }

                        var dateStay = request.CheckOut - request.CheckIn;

                        decimal priceRoom = 0;

                        int dateMonth = dateStay.GetValueOrDefault().Days / 30;
                        int dateDay = dateStay.GetValueOrDefault().Days - (dateMonth * 30);
                        int dateHour = dateStay.GetValueOrDefault().Hours;

                        totalAmount += (dateMonth * existRoom.PriceByMonth)
                                        + (dateDay * existRoom.PriceByDay)
                                        + (dateHour * existRoom.PriceByHour);
                    }
                }
                order.TotalAmount = totalAmount;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(OrderRepository)} GetAll Method has generated an error");
                return false;
            }
        }

        public async Task<bool> UpdateOrder(UpdateOrderCustomer request)
        {
            try
            {
                var orderExisting = await _dbSet.FindAsync(request.Id);

                if (orderExisting == null)
                {
                    return false;
                }

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

                orderExisting.CustomerName = request.CustomerName;
                orderExisting.Email = request.Email;
                orderExisting.PhoneNumber = request.PhoneNumber;
                orderExisting.Adress = request.Adress;
                orderExisting.IdentityCard = request.IdentityCard;
                orderExisting.Sex = request.Sex;
                orderExisting.Payment = request.Payment;
                orderExisting.CustomerId = user != null ? user.Id : "";
                orderExisting.Status = request.Status;
                orderExisting.RoomName = request.RoomName;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(OrderRepository)} UpdateOrder Method has generated an error");
                return false;
            }
        }


        public async Task<bool> AddOrder(CreateOrderRequestDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

                var order = new Order()
                {
                    CustomerName = request.CustomerName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Adress = request.Adress,    
                    IdentityCard = request.IdentityCard,
                    Sex = request.Sex,
                    CheckIn = request.CheckIn,
                    CheckOut = request.CheckOut,
                    Payment = request.Payment,
                    CustomerId = user != null ? user.Id : "",
                    Status = request.Status,
                    RoomName = request.RoomName,
                };

                decimal totalAmount = 0;

                if (request.OrderDetails?.Count > 0)
                {
                    foreach (var item in request.OrderDetails)
                    {
                        var existRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == item.RoomId);

                        if (existRoom == null)
                        {
                            return false;
                        }

                        var dateStay = request.CheckOut - request.CheckIn;

                        decimal priceRoom = 0;

                        int dateMonth = dateStay.GetValueOrDefault().Days / 30;
                        int dateDay = dateStay.GetValueOrDefault().Days - (dateMonth * 30);
                        int dateHour = dateStay.GetValueOrDefault().Hours;
 
                        var orderDetail = new OrderDetail()
                        {
                            RoomId = item.RoomId,
                            Order = order,
                            RoomName = existRoom.Name,
                            PriceRoom = priceRoom
                        };
                        totalAmount += (dateMonth * existRoom.PriceByMonth) 
                                        + (dateDay * existRoom.PriceByDay) 
                                        + (dateHour * existRoom.PriceByHour);

                        existRoom.Status = "Đã đặt";

                        await _context.OrderDetails.AddAsync(orderDetail);
                    }
                }
                order.TotalAmount = totalAmount;

                await _context.Orders.AddAsync(order);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(OrderRepository)} GetAll Method has generated an error");
                return false;
            }
        }
    }
}
