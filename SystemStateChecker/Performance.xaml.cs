using System;
using System.Windows;
using Microsoft.Win32;
using PerformanceTests;
using PerformanceTests.Tests;

namespace SystemStateChecker
{
    /// <summary>
    /// Логика взаимодействия для Performance.xaml
    /// </summary>
    public partial class Performance : Window
    {
        AntivirusTest at = new AntivirusTest();
        private event EventHandler TestCompleted;
        public Performance()
        {
            InitializeComponent();
            CopyTest copyTest = null;
            CopyStatsListBox.DataContext = TestsResult.CopyTestList;
            OpenDocStatsListBox.DataContext = TestsResult.OpenDocTestList;
            DownloadStatsListBox.DataContext = TestsResult.DownloadTestList;

            createCopyTestButton.Click += (s, e) =>
            {
                copyTest = new CopyTest(100000, 50);
                copyTestButton.IsEnabled = true;
            };
            copyTestButton.Click += (s, e) =>
            {
                if (!copyTest.IsNull)
                {
                    EnableAVMessage();
                    copyTest.Start();
                    copyTest.Start();
                    var withAV = copyTest.Result;
                    DisableAVMessage();
                    copyTest.Start();
                    copyTest.Start();
                    var withoutAV = copyTest.Result;
                    TestsResult.CopyTestList.Add(withAV.TotalMilliseconds/withoutAV.TotalMilliseconds, TestsResult.SystemInfo);
                    CopyStatsListBox.Items.Refresh();
                    //TestCompleted.Invoke(this,EventArgs.Empty);
                }
            };
            searchDocTestButton.Click += (s, e) =>
            {
                var ofd = new OpenFileDialog() {Filter = "Файлы Word|*.doc;*.docx", Title = "Выберите документ для проверки"};
                if (ofd.ShowDialog().Value)
                {
                    openDocTextBox.Text = ofd.FileName;
                    openDocTestButton.IsEnabled = true;
                }
            };
            openDocTestButton.Click += (s, e) =>
            {
                var odt = new OpenDocTest(openDocTextBox.Text);
                EnableAVMessage();
                odt.Start();
                var withAV = odt.Result;
                DisableAVMessage();
                odt.Start();
                var withoutAV = odt.Result;
                TestsResult.OpenDocTestList.Add(withAV.TotalMilliseconds / withoutAV.TotalMilliseconds, TestsResult.SystemInfo);
                OpenDocStatsListBox.Items.Refresh();
               // TestCompleted.Invoke(this, EventArgs.Empty);
            };
            downloadTestButton.Click += (s, e) =>
            {
                var dt = new DownloadTest();
                dt.DownloadClient.DownloadProgressChanged += (sender, args) =>{downloadProgressBar.Value = args.ProgressPercentage; }; 
                EnableAVMessage();
                dt.Start();
                downloadTestButton.IsEnabled = false;
                TimeSpan withAV;
                dt.DownloadClient.DownloadFileCompleted += (sender, args) =>
                {
                    dt.Dispose();
                    withAV = dt.Result;
                    var dt2 = new DownloadTest();
                    dt2.DownloadClient.DownloadProgressChanged += (sen, argss) => { downloadProgressBar.Value = argss.ProgressPercentage; };
                    DisableAVMessage();
                    dt2.Start();
                    TimeSpan withoutAV;
                    dt2.DownloadClient.DownloadFileCompleted += (obj, arg) =>
                    {
                        withoutAV = dt2.Result;
                        dt2.Dispose();
                        TestsResult.DownloadTestList.Add(withAV.TotalMilliseconds / withoutAV.TotalMilliseconds, TestsResult.SystemInfo);
                        DownloadStatsListBox.Items.Refresh();
                        downloadTestButton.IsEnabled = true;
                        downloadProgressBar.Value = 0;
                    };
                };

                //     TestCompleted.Invoke(this, EventArgs.Empty);
            };
            this.Closed += (s, e) =>
            {
                if (copyTest!=null) copyTest.Dispose();
            };
            this.TestCompleted += (s, e) =>
            {
                

            };
        }




        private void EnableAVMessage()
        {
            while (!at.State)
            {
                if (MessageBox.Show("Включите антивирус для проведения проверки", "Внимание!", MessageBoxButton.OKCancel,
                        MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
            }
        }

        private void DisableAVMessage()
        {
            while (at.State)
            {
                if (MessageBox.Show("Выключите антивирус для проведения проверки", "Внимание!", MessageBoxButton.OKCancel,
                        MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
            }
        }

    }
}
