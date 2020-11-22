using LOG430_TP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP.Models.StatisticComputers
{
    public class MeanComputer2 : IStatisticComputer<float, float>
    {
        public float Compute(IEnumerable<float> values)
        {
            return values.Sum() / values.Count();
        }
    }
}
