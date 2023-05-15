using AutoMapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Outgoing.Generic;
using HotelManager.Entities.Dto.Outgoing.UserDto;
using HotelManager.Entities.Message;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserInformationController : BaseController
    {
        public UserInformationController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userManager,
                                IMapper _mapper
                                )
                                : base(unitOfWork, userManager, _mapper)
        {
        }

        [HttpGet("GetUserCurrent")]
        public async Task<IActionResult> GetUserCurrent()
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);

            //var token = HttpContext.Request.Headers.Authorization;

            var result = new Result<UserDto>();

            if (loggedUser == null)
            {
                result.Error = PopulateError(400,
                                          ErrorMessage.UserMessage.UserNotFound,
                                          ErrorMessage.Generic.TypeBadRequest);
                return BadRequest(result);
            }

            var mapper = _mapper.Map<UserDto>(loggedUser);

            result.Content = mapper;
            result.IsSuccess = true;
            return Ok(result);
            //return Ok(token);
        }
    }
}
