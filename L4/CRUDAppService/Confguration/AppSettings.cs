﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAppService.Confguration
{
    public class AppSettings
    {
        public string DefaultLanguage { get; set; }
        public string BaseApiUrl { get; set; }

        public ProductEndpoint ProductEndpoint { get; set; }
    }
}
