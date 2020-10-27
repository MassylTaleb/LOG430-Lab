using LOG430_TP.ViewModels;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP.Models.StatisticComputers
{
    public class StandardDeviationComputer : IStatisticComputer<float, float>
    {
        public float Compute(IEnumerable<float> values)
        {
            var valueMinusAverageTotal = 0.0;
            var average = values.Average();

            foreach (float value in values)
            {
                valueMinusAverageTotal += Math.Pow(value - average, 2);
            }
            return (float) Math.Sqrt(valueMinusAverageTotal / (values.Count()-1));
        }
    }
}
