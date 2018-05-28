using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Tests
{
    public class OpenDocTest : IDisposable
    {
        private readonly string _path;

        private TimeSpan result;

        public TimeSpan Result => result;

        public OpenDocTest(string path)
        {
            _path = path;
        }




        public void Start()
        {
            var startTime = DateTime.Now;
            var doc = Process.Start(_path);
            result = DateTime.Now - startTime;
            doc.Kill();
        }

        public void Dispose()
        {
        }
    }
}
