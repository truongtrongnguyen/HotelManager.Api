using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Authentication.Models.Outgoing
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}
