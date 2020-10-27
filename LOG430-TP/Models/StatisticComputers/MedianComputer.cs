using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOG430_TP.ViewModels;

namespace LOG430_TP.Models.StatisticComputers
{
    public class MedianComputer : IStatisticComputer<float, float>
    {

        public float Compute(IEnumerable<float> values)
        {

            if (values.Count() == 1)
            {
                return values.First();
            }

            List<float> sortedValues = values.OrderBy(number=>number).ToList();


            if (sortedValues.Count % 2 == 0)
            {
                var firstValue = sortedValues[sortedValues.Count / 2];
                var secondValue = sortedValues[sortedValues.Count / 2 + 1];
                return (float) ((firstValue + secondValue) / 2.0);
            }

            return sortedValues[(sortedValues.Count + 1) / 2];
        }
    }
}
