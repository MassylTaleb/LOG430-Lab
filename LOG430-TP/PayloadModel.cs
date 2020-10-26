using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    class PayloadModel
    {
        public DateTime CreateUtc { get; set; }
        public string Unit { get; set; }
        public object Value { get; set; }
    }
}
