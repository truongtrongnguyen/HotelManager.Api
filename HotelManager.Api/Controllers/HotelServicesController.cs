using AutoMapper;
using HotelManage.Authentication.Models.Dto.Outgoing;
using HotelManager.DataService.Data;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class HotelServicesController : BaseController
    {
        public HotelServicesController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                 IMapper _mapper
                                )
                                : base(unitOfWork, userManager, _mapper)
        {
        }

        [HttpGet("GetAllHotelServices")]
        public async Task<IEnumerable<HotelService>> GetAll()
        {
            return await _unitOfWork.HotelServices.GetAll();
        }

        [HttpPost("CreateHotelService")]
        public async Task<IActionResult> CreateHotelService(CreateHotelService request)
        {
            if (ModelState.IsValid)
            {
                var hotelService = new HotelService()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Price = request.Price,
                    Quantity = request.Quantity
                };

                var isCreate = await _unitOfWork.HotelServices.Add(hotelService);

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

        [HttpPut("UpdateHotelService")]
        public async Task<IActionResult> UpdateHotelService(HotelService request)
        {
            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.HotelServices.Update(request);

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

        [HttpDelete("DeleteHotelService")]
        public async Task<IActionResult> DeleteHotelService(int id)
        {
            if (ModelState.IsValid)
            {
                var isDelete= await _unitOfWork.HotelServices.Delete(id);

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
