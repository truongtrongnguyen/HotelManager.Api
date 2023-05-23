using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.DbSet
{
    public class Employee
    {
        public string? Position { get; set; }
        public decimal? Allowvance { get; set; }
        public string?  BankNumber { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? DateContact { get; set; }
        public DateTime? ContactTerm { get; set; }
        [Key]
        public string IdentityId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
