using System;
using System.Collections.Generic;
using System.Management;
using System.Windows;
using SystemStateChecker.Tests;
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
        public Performance()
        {
            AntivirusTest at = new AntivirusTest();

            InitializeComponent();
            CopyTest copyTest = null;
            CopyStatsListBox.DataContext = TestsResult.CopyTestList;
            OpenDocStatsListBox.DataContext = TestsResult.OpenDocTestList;
            createCopyTestButton.Click += (s, e) =>
            {
                copyTest = new CopyTest(100000, 50);
                copyTestButton.IsEnabled = true;
            };
            copyTestButton.Click += (s, e) =>
            {
                if (!copyTest.IsNull)
                {
                    
                    while (!at.State)
                        {
                        if (MessageBox.Show("Включите антивирус для проведения проверки", "Внимание!", MessageBoxButton.OKCancel,
                                MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
                    }
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
                odt.Start();
                odt.Start();
                var withAV = odt.Result;
                do
                {
                    if (MessageBox.Show("Выключите антивирус для проведения проверки", "Внимание!", MessageBoxButton.OKCancel,
                            MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Cancel) return;
                } while (at.State);
                odt.Start();
                odt.Start();
                var withoutAV = odt.Result;
                TestsResult.OpenDocTestList.Add(withAV.TotalMilliseconds / withoutAV.TotalMilliseconds, $"{GetHardwareInfo("Win32_Processor", "Name")[0]}");
                OpenDocStatsListBox.Items.Refresh();
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
