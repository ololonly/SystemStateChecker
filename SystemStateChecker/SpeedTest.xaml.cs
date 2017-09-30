using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NSpeedTest;
using NSpeedTest.Models;

namespace SystemStateChecker
{
    /// <summary>
    /// Interaction logic for SpeedTest.xaml
    /// </summary>
    public partial class SpeedTest : Window
    {
        public SpeedTest()
        {
            InitializeComponent();
            closeButton.Click += (s, e) => this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<string>(s => SpeedInfoTextBox.Text += s);
            string result =
                await Task.Factory.StartNew<string>(() => SpeedTest.Check(progress), TaskCreationOptions.LongRunning);
            SpeedInfoTextBox.Text += result;
        }

        private static SpeedTestClient client;
        private static Settings settings;
        private const string DefaultCountry = "Russian Federation";

        public static string Check(IProgress<string> progress)
        {
            progress.Report("Получение настроек speedtest.net и списка серверов...");
            client = new SpeedTestClient();
            settings = client.GetSettings();

            var servers = SelectServers(progress);
            var bestServer = SelectBestServer(servers,progress);
            progress.Report("\nПроверка скорости...");
            var downloadSpeed = client.TestDownloadSpeed(bestServer, settings.Download.ThreadsPerUrl);
            PrintSpeed("Скачивание", downloadSpeed, progress);
            var uploadSpeed = client.TestUploadSpeed(bestServer, settings.Upload.ThreadsPerUrl);
            PrintSpeed("Загрузка", uploadSpeed, progress);
            return "\n\nУспешно!";
        }

        private static Server SelectBestServer(IEnumerable<Server> servers, IProgress<string> progress)
        {
            progress.Report("\nЛучший сервер:");
            var bestServer = servers.OrderBy(x => x.Latency).First();
            PrintServerDetails(bestServer, progress);
            progress.Report("\n"); 
            return bestServer;          
        }

        private static IEnumerable<Server> SelectServers(IProgress<string> progress)
        {
            progress.Report("\nВыбор ближайших серверов...");
            var servers = settings.Servers.Where(s => s.Country.Equals(DefaultCountry)).Take(8).ToList();
            foreach (var server in servers)
            {
                server.Latency = client.TestServerLatency(server);
                PrintServerDetails(server, progress);
            }
            progress.Report("\n");
            return servers;
        }

        private static void PrintServerDetails(Server server,IProgress<string> progress)
        {
            progress.Report($"\nХост {server.Sponsor} ({server.Name}/{server.Country}), расстояние: {(int)server.Distance / 1000}км, задержка: {server.Latency}мс");
        }

        private static void PrintSpeed(string type, double speed,IProgress<string> progress)
        {
            progress.Report(speed > 1024
                ? $"\n{type} скорость: {Math.Round(speed / 1024, 2)} Мб/с"
                : $"\n{type} скорость: {Math.Round(speed, 2)} Кб/с");
        }

        
    }
}
