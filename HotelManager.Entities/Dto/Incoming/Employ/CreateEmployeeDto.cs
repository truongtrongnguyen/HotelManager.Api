using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Employ
{
    public class CreateEmployeeDto
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
    }
}
