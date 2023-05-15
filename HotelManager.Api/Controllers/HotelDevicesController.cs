using AutoMapper;
using HotelManage.Authentication.Models.Dto.Outgoing;
using HotelManager.DataService.Data;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelDevice;
using HotelManager.Entities.Dto.Incoming.HotelService;
using HotelManager.Entities.Dto.Outgoing.Generic;
using HotelManager.Entities.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class HotelDevicesController : BaseController
    {
        public HotelDevicesController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IMapper _mapper
                                )
                                : base(unitOfWork, userManager, _mapper)
        {
        }

        [HttpGet("GetAllHotelDevices")]
        public async Task<IActionResult> GetAll()
        {
            var result = new Result<IEnumerable<HotelDevice>>();
            result.IsSuccess = true;
            result.Content = await _unitOfWork.HotelDevices.GetAll();
            return Ok(result);
        }

        [HttpPost("CreateHotelDevice")]
        public async Task<IActionResult> CreateHotelDevice(CreateHotelDevice request)
        {
            var result = new Result<CreateHotelDevice>();

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

                    result.Content = request;
                    result.IsSuccess = true;

                    return Ok(result);
                }

                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.SomethingWentWrong,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateHotelDevice")]
        public async Task<IActionResult> UpdateHoteDevice(HotelDevice request)
        {
            var result = new Result<HotelDevice>();

            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.HotelDevices.Update(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = request;
                    result.IsSuccess = true;

                    return Ok(result);
                }

                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.SomethingWentWrong,
                                                 ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }
            else
            {
                result.Error = PopulateError(400,
                                                ErrorMessage.Generic.InvalidRequest,
                                                ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
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
