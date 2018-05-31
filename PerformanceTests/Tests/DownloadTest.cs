using System;
using System.Net;

namespace PerformanceTests.Tests
{
    public class DownloadTest
    {
        private const string download_path = @"http://styashov.ru/download/speedtest100.iso";
        private const string download_path_less = @"http://styashov.ru/download/1.exe";
        private const string fileName = "testFile";

        public WebClient DownloadClient { get; private set; }
        public int Percent { get; private set; }

        public async void Start()
        {
            
            Uri path = new Uri(download_path_less);
            var client = new WebClient();
            DownloadClient = client;
            client.DownloadFileAsync(path,fileName);

        }

    }
}
