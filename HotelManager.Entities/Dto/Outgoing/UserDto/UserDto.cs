using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Outgoing.UserDto
{
    public class UserDto
    {
        public string? FullName { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? IdentityCard { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Avata { get; set; }

    }
}
