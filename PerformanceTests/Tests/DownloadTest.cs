using System;
using System.IO;
using System.Net;

namespace PerformanceTests.Tests
{
    public class DownloadTest : IDisposable
    {
        private const string download_path = @"http://styashov.ru/download/speedtest100.iso";
        private const string download_path_less = @"http://styashov.ru/download/1.exe";
        private const string fileName = "testFile";
        private TimeSpan result;

        public WebClient DownloadClient { get; private set; } = new WebClient();
        public TimeSpan Result => result;
        public int Percent { get; private set; }

        public void Start()
        {   
            if (File.Exists(fileName)) File.Delete(fileName);
            Uri path = new Uri(download_path_less);
            var startTime = DateTime.Now;
            DownloadClient.DownloadFileAsync(path, fileName);
            result = DateTime.Now - startTime;
            DownloadClient.Dispose();
        }
        
        public void Dispose()
        {
            if (File.Exists(fileName)) File.Delete(fileName);
        }

    }
}
