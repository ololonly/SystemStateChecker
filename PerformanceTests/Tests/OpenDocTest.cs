using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

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
            var ap = new Application();
            Document doc = ap.Documents.Open(_path);
            doc.Activate();
            result = DateTime.Now - startTime;
            ap.Documents.Close();
            ap.Quit();
        }

        public void Dispose()
        {
        }
    }
}
