using System.Diagnostics;

namespace PerformanceTests
{
    public static class PerformanceStats
    {
        private static PerformanceCounter cpuCounter;
        public static PerformanceCounter CpuCounter => cpuCounter;

        static PerformanceStats()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            
        }

    }
}
