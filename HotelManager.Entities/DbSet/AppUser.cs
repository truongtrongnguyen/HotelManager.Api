
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.DbSet
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Avata { get; set; }
        public string? Address { get; set; }
        public string? IdentityCard { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime? DateUpdate { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsCustomer { get; set; }
    }
}
