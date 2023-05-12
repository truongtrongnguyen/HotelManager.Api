using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.User
{
    public class CreateUserDto
    {
        public string FullName { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? IdentityCard { get; set; }
    }
}
