﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class ApplicationMessage
    {
        public string Topic { get; set; }

        public string Payload { get; set; }

        public int QualityOfServiceLevel { get; set; }

        public bool Retain { get; set; }

        public DateTime date { get; set; }
    }
}
