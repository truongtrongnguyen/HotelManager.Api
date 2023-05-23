using AutoMapper;
using Dapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Room;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;

namespace HotelManager.Api.Controllers
{
    public class RoomDapperController : BaseController
    {
        private readonly IConfiguration _config;
        public RoomDapperController(IUnitOfWork unitOfWork,
                               UserManager<AppUser> userManager,
                               IMapper _mapper,
                               IConfiguration config
                               )
                               : base(unitOfWork, userManager, _mapper)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRoom()
        {
            using var connect = new SqlConnection(_config.GetConnectionString("AppDbContext"));

            var result = await _unitOfWork.DapperRooms.GetAll(connect);
            return Ok(result);
        }

        [HttpPost("CreateRoomDapper")]
        public async Task<IActionResult> CreateRoomDapper([FromForm] CreateRoomDapper request)
        {
            using var connect = new SqlConnection(_config.GetConnectionString("AppDbContext"));

            var result = await _unitOfWork.DapperRooms.CreateRoom(connect, request);

            return Ok(result);
        }

        [HttpPost("UpdateRoomDapper")]
        public async Task<ActionResult> UpdateRoomDapper([FromForm] CreateRoomDapper request)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("AppDbContext"));

            var query = new StringBuilder();

            query.Append("UPDATE Rooms ");
            query.Append("SET ");
            query.Append("Name = @Name, ");
            query.Append("Price = @Price, ");
            query.Append("Sizes = @Sizes, ");
            query.Append("Status = @Status, ");
            query.Append("PeoplNumber = @PeoplNumber, ");
            query.Append("PriceByHour = @PriceByHour, ");
            query.Append("PriceByDay = @PriceByDay, ");
            query.Append("PriceByMonth = @PriceByMonth ");
            query.Append("WHERE Id = @Id");

            var temp =query.ToString();

            var result = await connection.ExecuteAsync(temp, request);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoomDapper(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("AppDbContext"));
            var isDelete = await _unitOfWork.DapperRooms.DeleteRoomDapper(connection, Id);
            return Ok(isDelete);
        }

    }
}
