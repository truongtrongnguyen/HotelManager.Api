using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Photo
{
    public class DeletePhotoRequest
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
