﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostMVC.Models.Account
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
