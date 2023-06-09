﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Authentication.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; } = string.Empty;
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
