using System;
using System.Management;

namespace SystemStateChecker.Tests
{
    public class FirewallTest : ITest
    {
        public bool State { get; private set; }

        public string Result()
        {
            string result = string.Empty;
            try
            {
                //select the proper wmi namespace depending of the windows version
                string WMINameSpace = System.Environment.OSVersion.Version.Major > 5
                    ? "SecurityCenter2"
                    : "SecurityCenter";
                
                ManagementScope Scope;
                Scope = new ManagementScope(String.Format($"\\\\localhost\\root\\{WMINameSpace}"), null);

                Scope.Connect();
                ObjectQuery Query = new ObjectQuery("SELECT * FROM FirewallProduct");
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query);
                
                foreach (ManagementObject WmiObject in Searcher.Get())
                {
                    result = $"Название фаервола: {WmiObject["displayName"]}";
                    result += "\nСостояние: ";
                    if (System.Environment.OSVersion.Version.Major < 6) //is XP ?
                    {
                        result += $"\nВключен {WmiObject["enabled"]}";
                    }
                    else
                    {
                        var state = int.Parse(WmiObject["productState"].ToString());
                        var state1 = state.ToString("X");                        
                        State = state.ToString("X").Substring(1, 1).Equals("1") ? true : false;
                        result += State ? "Включен" : "Выключен";
                    }
                }
            }
            catch (Exception e)
            {
               result = $"Исключение { e.Message} ошибка {e.StackTrace}";
            }
            return result==string.Empty?"Фаервол не установлен":result;
        }
    }
}
