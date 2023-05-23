using AutoMapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelService;
using HotelManager.Entities.Dto.Incoming.Photo;
using HotelManager.Entities.Dto.Incoming.Room;
using HotelManager.Entities.Dto.Outgoing.Generic;
using HotelManager.Entities.Dto.Outgoing.Room;
using HotelManager.Entities.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class RoomsController : BaseController
    {
        public RoomsController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IMapper _mapper
                                )
                                : base(unitOfWork, userManager, _mapper)
        {
        }

        [HttpGet("GetAllRooms")]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _unitOfWork.Rooms.GetAlll();
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromForm]CreateRoom request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var room = await _unitOfWork.Rooms.AddRoom(request);

                if (room != null)
                {
                    if (request.Photos?.Count > 0)
                    {
                        foreach (var item in request.Photos)
                        {
                            var success = await _unitOfWork.Photos.AddPhoto(room, item);

                            if (!success)
                            {
                                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.SomethingWentWrong,
                                                 ErrorMessage.Generic.TypeBadRequest);
                                return BadRequest(result);
                            }
                        }
                    }

                    await _unitOfWork.CompleteAsync();

                    result.Content = "Create Room Success";
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

        [HttpPut("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(UpdateRoom request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Rooms.UpdateRoom(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "Update Room Success";
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
        

        // Add Image
        [HttpPut("AddImage")]
        public async Task<IActionResult> AddImage([FromForm] UpdatePhoto request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Photos.UpdatePhotos(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "AddImage Room Success";
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

        [HttpDelete("UpdatePhotoDelete")]
        public async Task<IActionResult> UpdatePhotoDelete(List<DeletePhotoRequest> request)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Photos.DeletePhotos(request);

                if (isCreate)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "UpdatePhotoDelete Room Success";
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

        [HttpDelete("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = new Result<string>();

            if (ModelState.IsValid)
            {
                var isDelete = await _unitOfWork.Rooms.Delete(id);

                if (isDelete)
                {
                    await _unitOfWork.CompleteAsync();

                    result.Content = "DeleteRoom Room Success";
                    result.IsSuccess = true;

                    return Ok(result);
                }

                result.Error = PopulateError(400,
                                                 ErrorMessage.Generic.InvalidRequest,
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
    }
}
