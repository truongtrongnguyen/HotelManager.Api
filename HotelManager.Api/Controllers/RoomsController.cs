using HotelManage.Authentication.Models.Outgoing;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelService;
using HotelManager.Entities.Dto.Incoming.Photo;
using HotelManager.Entities.Dto.Incoming.Room;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class RoomsController : BaseController
    {
        public RoomsController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager)
                                : base(unitOfWork, userManager)
        {
        }

        [HttpGet("GetAllRooms")]
        public async Task<IEnumerable<dynamic>> GetAllRooms()
        {
            return await _unitOfWork.Rooms.GetAlll();
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromForm]CreateRoom request)
        {
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
                                BadRequest("Something went wrong, please try again latter ");
                            }
                        }
                    }

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

        [HttpPut("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(UpdateRoom request)
        {
            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Rooms.UpdateRoom(request);

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
        

        // Add Image
        [HttpPut("AddImage")]
        public async Task<IActionResult> AddImage([FromForm] UpdatePhoto request)
        {
            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Photos.UpdatePhotos(request);

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

        [HttpDelete("UpdatePhotoDelete")]
        public async Task<IActionResult> UpdatePhotoDelete(List<DeletePhotoRequest> request)
        {
            if (ModelState.IsValid)
            {
                var isCreate = await _unitOfWork.Photos.DeletePhotos(request);

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

        [HttpDelete("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (ModelState.IsValid)
            {
                var isDelete = await _unitOfWork.Rooms.Delete(id);

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
