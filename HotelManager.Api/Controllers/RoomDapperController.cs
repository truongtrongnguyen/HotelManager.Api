using AutoMapper;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Api.Controllers
{
    public class RoomDapperController : BaseController
    {
        public RoomDapperController(IUnitOfWork unitOfWork,
                               UserManager<AppUser> userManager,
                               IMapper _mapper
                               )
                               : base(unitOfWork, userManager, _mapper)
        {
        }        
    }
}
