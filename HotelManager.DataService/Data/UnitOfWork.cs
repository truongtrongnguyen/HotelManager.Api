using HotelManager.DataService.IConfiguration;
using HotelManager.DataService.IRepository;
using HotelManager.DataService.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private ILogger _logger;

        public IUserRepository Users { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IHotelServiceRepository HotelServices { get; private set; }
        public IHotelDeviceRepository HotelDevices { get; private set; }
        public IRoomRepository Rooms { get; private set; }  
        public IPhotosRepository Photos { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }

        // Dapper
        public IDapperRoomRepository DapperRooms { get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");

            Users = new UserRepository(_context, _logger);
            Employees = new EmployeeRepository(_context, _logger);
            HotelServices = new HotelServiceRepository(_context, _logger);
            HotelDevices = new HotelDeviceRepository(_context, _logger);
            Rooms = new RoomRepository(_context, _logger);
            Photos = new PhotoRepository(_context, _logger);
            Orders = new OrderRepository(_context, _logger);
            RefreshTokens = new RefreshTokenRepository(_context, _logger);

            // Dapper
            DapperRooms = new DapperRoomRepository(_logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
