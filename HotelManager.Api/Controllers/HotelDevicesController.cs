using HotelManage.Authentication.Models.Outgoing;
using HotelManager.DataService.Data;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelDevice;
using HotelManager.Entities.Dto.Incoming.HotelService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class HotelDevicesController : BaseController
    {
        public HotelDevicesController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager)
                                : base(unitOfWork, userManager)
        {
        }

        [HttpGet("GetAllHotelDevices")]
        public async Task<IEnumerable<HotelDevice>> GetAll()
        {
            return await _unitOfWork.HotelDevices.GetAll();
        }

        [HttpPost("CreateHotelDevice")]
        public async Task<IActionResult> CreateHotelDevice(CreateHotelDevice request)
        {
            if (ModelState.IsValid)
            {
                var hotelDevice = new HotelDevice()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Quantity = request.Quantity
                };

                var isCreate = await _unitOfWork.HotelDevices.Add(hotelDevice);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    return Ok(request);
                }

                return BadRequest("Something went wrong, please try again latter ");
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpPut("UpdateHotelDevice")]
        public async Task<IActionResult> UpdateHoteDevice(HotelDevice request)
        {
            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.HotelDevices.Update(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    return Ok(request);
                }

                return BadRequest("Something went wrong, please try again latter ");
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpDelete("DeleteHotelDevice")]
        public async Task<IActionResult> DeleteHotelDevice(int id)
        {
            if (ModelState.IsValid)
            {
                var isDelete= await _unitOfWork.HotelDevices.Delete(id);

                if (isDelete)
                {
                    await _unitOfWork.CompleteAsync();

                    return Ok();
                }

                return BadRequest("Something went wrong, please try again latter ");
            }
            else
            {
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }
    }
}
