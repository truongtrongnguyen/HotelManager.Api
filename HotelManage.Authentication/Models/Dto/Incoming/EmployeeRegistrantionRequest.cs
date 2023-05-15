using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Authentication.Models.Dto.Incoming
{
    public class EmployeeRegistrantionRequest
    {
        public string FullName { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public decimal? Allowvance { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? DateContact { get; set; }
        public DateTime? ContactTerm { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? IdentityCard { get; set; }
        public string? BankNumber { get; set; }
    }
}
