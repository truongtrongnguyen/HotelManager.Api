using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Authentication.Models.Dto.Incoming
{
    public class TokenRequestDto
    {
        [Required]        
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
