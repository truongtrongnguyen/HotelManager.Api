using HotelManager.Entities.Dto.Outgoing.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Dto.Outgoing.Generic
{
    public class Result<T>
    {
        public T Content { get; set; }
        public Error Error { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
