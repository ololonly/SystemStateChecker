using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests
{
    static class PerformanceStats
    {
        private static PerformanceCounter cpuCounter;
        public static PerformanceCounter CpuCounter => cpuCounter;

        static PerformanceStats()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

    }
}
