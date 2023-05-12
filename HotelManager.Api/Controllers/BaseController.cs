using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
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

        public BaseController(IUnitOfWork unitOfWork,
                                UserManager<AppUser> userNabager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userNabager;
        }
    }
}
