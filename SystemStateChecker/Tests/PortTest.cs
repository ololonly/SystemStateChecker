using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SystemStateChecker.Tests
{


    public struct Port
    {
        public int Number { get; }
        public string Description { get; }

        public Port(int port, string description)
        {
            Number = port;
            Description = description;
        }
    }


    public class PortTest : ITest
    {
        public event EventHandler PortChecked;
        public bool State { get; private set; }

        public string Result()
        {
            if (State) return "Все порты закрыты";
            else
            {
                string result = "Порты ";
                foreach (var openedPort in openedPorts)
                {
                    result += $"{openedPort.Number} ";
                }
                result += "открыты.";
                return result;
            }
        }
        
        public int TestsCount => ports.Count - 1;

        private readonly List<Port> ports = new List<Port>()
        {
            new Port(20,"FTP"),
            new Port(21,"FTP"),
            new Port(22,"SSH"),
            new Port(23,"telnet"),
            new Port(25,"SMTP"),
            new Port(42,"WINS"),
            new Port(43,"WHOIS"),
            new Port(53,"DNS"),
            new Port(67,"DHCP"),
            new Port(69,"TFTP"),
            new Port(80,"HTTP"),
            new Port(110,"POP3"),
            new Port(115,"SFTP"),
            new Port(123,"NTP"),
            new Port(137,"NetBIOS"),
            new Port(138,"NetBIOS"),
            new Port(139,"NetBIOS"),
            new Port(143,"IMAP"),
            new Port(161,"SMNP"),
            new Port(179,"BGP"),
            new Port(443,"HTTPS"),
            new Port(445,"SMB"),
            new Port(514,"Syslog"),
            new Port(515,"LPD"),
            new Port(993,"IMAP SSL"),
            new Port(995,"POP3 SSL"),
            new Port(1080,"SOCKS"),
            new Port(1194,"OpenVPN"),
            new Port(1433,"MSSQL"),
            new Port(1702,"L2TP"),
            new Port(1723,"PPTP"),
            new Port(3128,"Proxy"),
            new Port(3268,"LDAP"),
            new Port(3306,"MySQL"),
            new Port(3389,"RDP"),
            new Port(5432,"PostgreSQL"),
            new Port(5900,"VNC"),
            new Port(5938,"TeamViewer"),
            new Port(8080,"HTTP/Web"),
            new Port(10000,"NDMP"),
            new Port(20000,"DNP")
        };
        private List<Port> openedPorts = new List<Port>();
        private readonly string currIp;

        public PortTest(bool ipType)
        {
            currIp = ip(ipType);
        }

        private string ip(bool type)
        {
            return type? new WebClient().DownloadString("https://api.ipify.org"):Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        public string Check(IProgress<string> progress)
        {
            State = true;
            progress.Report($"Проверка {currIp}\n");
            foreach (var port in ports)
            {
                {
                    TcpClient TcpScan = new TcpClient();
                    try
                    {
                        
                        // Try to connect
                        TcpScan.Connect(currIp,port.Number);
                        State = false;
                        openedPorts.Add(port);
                        // If there's no exception, we can say the port is open
                        progress.Report($"Port {port.Number} - {port.Description} is opened\n");
                    }
                    catch
                    {
                        // An exception occured, thus the port is probably closed
                        progress.Report($"Port {port.Number} - {port.Description} is closed\n");
                    }
                    PortChecked.Invoke(this,null);
                }
            }
            return "End";
        }
    }
}
