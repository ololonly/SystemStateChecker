using System;
using System.Collections.Generic;
using System.Management;
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
                    do
                    {
                        if (MessageBox.Show("Выключите антивирус для проведения проверки", "Внимание!", MessageBoxButton.OKCancel,
                                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
                    } while (!at.State);
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
                    TestsResult.CopyTestList.Add(withAV.TotalMilliseconds/withoutAV.TotalMilliseconds, $"{GetHardwareInfo("Win32_Processor", "Name")[0]}");
                    CopyStatsListBox.Items.Refresh();
                }
            };
            this.Closed += (s, e) =>
            {
                if (copyTest!=null) copyTest.Dispose();
            };
        }

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
