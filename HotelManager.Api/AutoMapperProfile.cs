using AutoMapper;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.HotelDevice;
using HotelManager.Entities.Dto.Outgoing.Employee;
using HotelManager.Entities.Dto.Outgoing.UserDto;

namespace HotelManager.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserDto>();
            CreateMap<AppUser, EmployeeDto>();
        }
    }
}
