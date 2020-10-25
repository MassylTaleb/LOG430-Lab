using System.Collections;
using System.Collections.Generic;

namespace LOG430_TP.ViewModels
{
    public interface IStatisticComputer<T1, T2>
    {
        T2 Compute(IEnumerable<T1> collection);
    }
}