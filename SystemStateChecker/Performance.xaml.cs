using System;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using SystemStateChecker.Tests;
using PerformanceTests;
using PerformanceTests.Tests;

namespace SystemStateChecker
{
    /// <summary>
    /// Логика взаимодействия для Performance.xaml
    /// </summary>
    public partial class Performance : Window
    {
        public Performance()
        {
            InitializeComponent();
            CopyTest copyTest = null;
            CopyStatsListBox.DataContext = TestsResult.CopyTestList;
            createCopyTestButton.Click += (s, e) =>
            {
                copyTest = new CopyTest(100000, 50);
                copyTestButton.IsEnabled = true;
            };
            copyTestButton.Click += (s, e) =>
            {
                if (!copyTest.IsNull)
                {
                    var at = new SystemStateChecker.Tests.AntivirusTest();
                    copyTest.Start();
                    copyTest.Start();
                    var withAV = copyTest.Result;
                    do
                    {
                       if (MessageBox.Show("Выключите антивирус для проведения проверки","Внимание!",MessageBoxButton.OKCancel,
                               MessageBoxImage.Information,MessageBoxResult.OK,MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
                    } while (at.State);
                    copyTest.Start();
                    copyTest.Start();
                    var withoutAV = copyTest.Result;
                    TestsResult.CopyTestList.Add(withAV.TotalMilliseconds/withoutAV.TotalMilliseconds, "Intel Core i7 7700k");
                    CopyStatsListBox.Items.Refresh();
                }
            };
        }
    }
}
