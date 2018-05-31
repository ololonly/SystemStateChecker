using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Tests
{
    public class DownloadTest
    {
        private const string download_path = @"http://styashov.ru/download/speedtest100.iso";
        private const string download_path_less = @"http://styashov.ru/download/1.exe";
        private const string fileName = "testFile";

        public int Percent { get; private set; }

        public async void Start()
        {
            
            Uri path = new Uri(download_path_less);
            var client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, args) =>
            {
                Percent = args.ProgressPercentage;
            });
            client.DownloadFileAsync(path,fileName);

        }

    }
}
