using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests
{
    public static class TestsResult
    {
        private static SortedList<double,string> copyTestList = new SortedList<double, string>();

        public static SortedList<double, string> CopyTestList
        {
            get => copyTestList;
            set => copyTestList = value;
        }
    }
}
