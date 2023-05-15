using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Outgoing.Room
{
    public class RoomResponse
    {
        public RoomResponse()
        {
            Images = new List<ImageTemp>();
        }
        public string? Name { get; set; }
        public float? Sizes { get; set; }
        public decimal? Price { get; set; }
        public string? Desciption { get; set; }
        public string? Status { get; set; }
        public int? PeoplNumber { get; set; }
        public decimal PriceByHour { get; set; }
        public decimal PriceByDay { get; set; }
        public decimal PriceByMonth { get; set; }
        public List<ImageTemp> Images { get; set; }
    }
}
