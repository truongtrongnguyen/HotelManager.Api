using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Room;
using HotelManager.Entities.Dto.Outgoing.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManager.DataService.Repository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<IEnumerable<RoomResponse>> GetAlll()
        {
            try
            {
                var rooms = await _dbSet.Include(x => x.HotelPhotos)
                                    .AsNoTracking().ToListAsync();

                var result = new List<RoomResponse>();
                foreach(var room in rooms)
                {
                    var temp = new RoomResponse()
                    {
                        Name = room.Name,
                        Sizes = room.Sizes,
                        Desciption = room.Desciption,
                        Price = room.Price,
                        PeoplNumber = room.PeoplNumber, 
                        Status = room.Status
                    };
                    room.HotelPhotos.ForEach(x => temp.Images.Add(new ImageTemp() { Id = x.Id, ImageUrl = x.PhotoName }));
                    result.Add(temp);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RoomRepository)} GetAll Method has generated an error");
                return new List<RoomResponse>();
            }
        }

        public async Task<Room> AddRoom(CreateRoom request)
        {
            try
            {
                var room = new Room()
                {
                    Name = request.Name,
                    Sizes = request.Sizes,
                    Desciption = request.Desciption,
                    Price = request.Price,
                    PeoplNumber = request.PeoplNumber,
                    Status = request.Status
                };

                await _dbSet.AddAsync(room);

                if(request.Photos?.Count > 0)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhotosRoom");
                    if (!Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }
                }

                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RoomRepository)} AddRoom Method has generated an error");
                return null;
            }
        }

        public async Task<bool> UpdateRoom(UpdateRoom room)
        {
            try
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == room.Id);

                if (existing == null)
                {
                    return false;
                }
                // Update
                existing.Name = room.Name;
                existing.Desciption = room.Desciption;
                existing.Status = room.Status;
                existing.Price = room.Price;
                existing.Sizes = room.Sizes;
                existing.PeoplNumber = room.PeoplNumber;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RoomRepository)} GetAll Method has generated an error");
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (existing == null)
                {
                    return false;
                }

                _dbSet.Remove(existing);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RoomRepository)} Delete Method has generated an error");
                return false;
            }
        }
    }
}
