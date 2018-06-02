using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using PerformanceTests.Tests;

namespace PerformanceTests
{
    public static class TestsResult
    {
        public static SortedList<double, string> CopyTestList { get; set; } = new SortedList<double, string>();

        public static SortedList<double, string> OpenDocTestList { get; set; } = new SortedList<double, string>();

        public static SortedList<double, string> DownloadTestList { get; set; } = new SortedList<double, string>();

        static TestsResult()
        {
            var avt = new AntivirusTest();
            SystemInfo= $"{GetHardwareInfo("Win32_Processor", "Name")[0]} {avt.CurrentAv()}";
        }


        public static string SystemInfo { get; }


        private static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            List<string> result = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result.Add(obj[ClassItemField].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
