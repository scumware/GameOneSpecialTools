using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadTester
{
    public enum LoadType
    {
        YieldExecution,
        Sleep,
        EmptyLoop,
        SpinWait,
        MemoryPressure,
        PrepareSampleHistogramm50k
    }
}
