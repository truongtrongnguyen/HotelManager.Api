using Dapper;
using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Room;
using HotelManager.Entities.Dto.Outgoing.Room;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManager.DataService.Repository
{
    public class DapperRoomRepository : IDapperRoomRepository
    {
        private readonly ILogger _logger;
        public DapperRoomRepository(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Room>> GetAll(SqlConnection connect)
        {
            try
            {
                var roomQuery = new StringBuilder();

                roomQuery.Append("SELECT * ");
                roomQuery.Append("FROM Rooms ");

                var imageQuery = "SELECT PhotoName, Id FROM Photos where RoomId = @Id";

                var result = await connect.QueryAsync<Room>(roomQuery.ToString());

                var roomList = new List<RoomResponse>();

                foreach (var room in result)
                {
                    //var roomResponse = new RoomResponse()
                    //{
                    //    Name = room.Name,
                    //    Price = room.Price,
                    //    Sizes = room.Sizes,
                    //    Status = room.Status,
                    //    Desciption = room.Desciption,
                    //    PriceByDay = room.PriceByDay,
                    //    PeoplNumber = room.PeoplNumber,
                    //    PriceByHour = room.PriceByHour,
                    //    PriceByMonth = room.PriceByMonth,
                    //};
                    //var image = await connect.QueryAsync<ImageTemp>(imageQuery, new {Id = room.Id  });
                    //roomResponse.Images = image.ToList();

                    //roomList.Add(roomResponse);
                    var image = await connect.QueryAsync<Photos>("Select * from Photos where RoomId = @Id", new { Id = room.Id });
                    room.HotelPhotos = image.ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(DapperRoomRepository)} GetAll Method has generate an error");
                return new List<Room>();
            }
        }


        public async Task<bool> CreateRoom(SqlConnection connection, CreateRoomDapper request)
        {
            try
            {
                var query = new StringBuilder();

                query.Append("INSERT INTO ");
                query.Append("Rooms (Name, Price, Sizes, Desciption, Status, PeoplNumber, PriceByHour, PriceByDay, PriceByMonth, DateCreate) ");
                query.Append("Values (@Name, @Price, @Sizes, @Desciption, @Status, @PeoplNumber, @PriceByHour, @PriceByDay, @PriceByMonth, @DateCreate) ");

                var result = await connection.ExecuteAsync(query.ToString(), request);

                if(result > 0)
                {
                    var room = await connection.QueryAsync<int>("SELECT Id FROM Rooms where Name = @Name ",
                                                                new { Name = request.Name });

                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhotosRoom");

                    if (!Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }

                    if (request.Photos?.Count > 0)
                    {
                        foreach (var item in request.Photos)
                        {
                            var fileName = Path.GetRandomFileName() + Path.GetExtension(item.FileName);
                            var file = Path.Combine(directory, fileName);

                            using (var streamFile = new FileStream(file, FileMode.Create))
                            {
                                await item.CopyToAsync(streamFile);
                            }
                            var photo = new Photos()
                            {
                                RoomId = room.First(),
                                PhotoName = "/Contents/" + "PhotosRoom/" + fileName,
                                PhotoUrl = file
                            };
                            var addPhoto = await connection.ExecuteAsync("INSERT INTO Photos (RoomId, PhotoName, PhotoUrl)" +
                                                                         " VALUES (@RoomId, @PhotoName, @PhotoUrl) ", photo);
                            if (addPhoto == 0)
                            {
                                return false;
                            }

                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(DapperRoomRepository)} Create Room method has generated an error");
                return false;
            }
        }

        public async Task<bool> DeleteRoomDapper(SqlConnection connection, int Id)
        {
            try
            {
                var roomExisting = await connection.QueryFirstAsync<Room>("Select * from Rooms where Id = @Id", new {Id = Id});

                if (roomExisting == null)
                {
                    return false;
                }
                var image = await connection.QueryAsync<string>("Select PhotoUrl from Photos where RoomId = @Id", new { Id = Id });

                if (image.Any())
                {
                    foreach (var item in image)
                    {
                        System.IO.File.Delete(item);
                    }
                }

                var deleteroom = await connection.ExecuteAsync("Delete Rooms where Id = @Id", new { Id = Id });
                if (deleteroom > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(DapperRoomRepository)} DeleteRoomDapper method has generated an error");
                return false;
            }
        }
    }
}
