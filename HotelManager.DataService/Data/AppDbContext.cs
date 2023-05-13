using HotelManager.Entities.DbSet;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<HotelDevice> HotelDevices { get; set; }
        public virtual DbSet<HotelService> HotelService { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

    }
}
