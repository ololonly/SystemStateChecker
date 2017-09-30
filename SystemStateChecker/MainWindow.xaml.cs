using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SystemStateChecker.Tests;

namespace SystemStateChecker
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            connectionTestButton.Click += (s, e) => {
                var ct = new ConnectionTest("google.com");
                connectionDataTextBox.Text = ct.Result();
                connectionDataTextBox.Background = ct.State
                    ? Brushes.LimeGreen
                    : Brushes.Red;
                speedTestButton.IsEnabled = ct.State;
            };
            speedTestButton.Click += (s, e) => { var st = new SpeedTest(); st.Show(); };
            firewallTestButton.Click += (s, e) =>{
                var ft = new FirewallTest();
                firewallDataTextBox.Text = ft.Result();
                firewallDataTextBox.Background = ft.State ? Brushes.LimeGreen : Brushes.Red;
            };
            antivirusTestButton.Click += (s, e) =>
            {
                var at = new AntivirusTest();
                antivirusDataTextBox.Text = at.Result();
                antivirusDataTextBox.Background = at.State ? Brushes.LimeGreen : Brushes.Red;
            };
            securityTestButton.Click += async (s, e) =>{
                var st = new SecurityTest();
                securityDataTextBox.Text= await st.ResultAsync();
                securityDataTextBox.Background = st.State ? Brushes.LimeGreen : Brushes.Red;
            };
            logCreationButton.Click += (s, e) =>
            {
                var tbs = FindVisualChildren<TextBox>(this);
                if (File.Exists("log.txt")) File.Delete("log.txt");
                using (FileStream fs = File.Create("log.txt"))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        foreach (var item in tbs)
                        {
                            if (!item.Text.Equals(string.Empty)) sw.WriteLine(item.Text);
                        }
                    }                   
                }
            };
            IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
            {
                if (depObj != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                        if (child != null && child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }
    }
}
