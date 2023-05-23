using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Room
{
    public class CreateRoomDapper
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float? Sizes { get; set; }
        public decimal? Price { get; set; }
        public string? Desciption { get; set; }
        public string? Status { get; set; }
        public int? PeoplNumber { get; set; }
        public decimal PriceByHour { get; set; }
        public decimal PriceByDay { get; set; }
        public decimal PriceByMonth { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public List<IFormFile>? Photos { get; set; }
    }
}
