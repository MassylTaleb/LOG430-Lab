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

                /*            values.Sort();

                            if (values.Count == 1)
                            {
                                return values.First() + firstPayload.Unit;
                            }

                            if (values.Count % 2 == 0)
                            {
                                var firstValue = values[values.Count / 2];
                                var secondValue = values[values.Count / 2 + 1];
                                return (firstValue + secondValue) / 2.0 + firstPayload.Unit;
                            }

                            return values[(values.Count + 1) / 2] + firstPayload.Unit;*/
                float test = 1;
                return test;
            }
        

        
    }
}
