using System;
using System.Data.SqlTypes;
using System.IO;

namespace PerformanceTests.Tests
{
    public class CopyTest : IDisposable, INullable
    {
        private readonly int sizeOfFile;
        private readonly int numberOfOperations;
        private const string path = "copyTestFile.txt";
        private TimeSpan result;

        public TimeSpan Result => result;

        public CopyTest(int size, int count)
        {
            sizeOfFile = size;
            numberOfOperations = count;
            write();
        }

        private void write()
        {
            File.Create(path).Dispose();
            using (var sw = new StreamWriter(path))
            {
                for (int i = 0; i < sizeOfFile; i++)
                {
                   sw.Write("1");
                }
            }
        }

        public void copy()
        {
            for (int i = 0; i < numberOfOperations; i++)
            {
                using (var fs = new FileStream(path,FileMode.Open))
                {
                    fs.CopyTo(fs);
                }
            }
        }

        public void Start()
        {
            var startTime = DateTime.Now;
            copy();
            result = DateTime.Now - startTime;
        }

        public void Dispose()
        {
            if (File.Exists(path)) File.Delete(path);
        }

        public bool IsNull => !File.Exists(path);
    }
}
