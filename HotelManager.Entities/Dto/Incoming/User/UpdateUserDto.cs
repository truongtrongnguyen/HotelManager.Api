using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.User
{
    public class UpdateUserDto
    {
        public string? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? IdentityCard { get; set; }
        public IFormFile? Avata { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
