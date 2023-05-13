using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Incoming.Photo
{
    public class UpdatePhoto
    {
        public int id { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
