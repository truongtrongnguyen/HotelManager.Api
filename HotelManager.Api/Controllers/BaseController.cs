using AutoMapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Outgoing.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly UserManager<AppUser> _userManager;
        public readonly IMapper _mapper;

        public BaseController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userNabager,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userNabager;
            _mapper = mapper;
        }

        internal Error PopulateError(int code, string message, string type)
        {
            return new Error()
            {
                Code = code,
                Message = message,
                Type = type
            };
        }
    }
}
