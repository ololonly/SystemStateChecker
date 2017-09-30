using System.Management;

namespace SystemStateChecker.Tests
{
    public class AntivirusTest : ITest
    {
        public bool State { get; private set; }
        public string Result()
        {
            State=false;
            string result = string.Empty;
            ConnectionOptions _connectionOptions = new ConnectionOptions{EnablePrivileges = true, Impersonation = ImpersonationLevel.Impersonate};
            ManagementScope _managementScope = new ManagementScope($"\\\\localhost\\root\\SecurityCenter2", _connectionOptions);
            _managementScope.Connect();
            ObjectQuery _objectQuery = new ObjectQuery("SELECT * FROM AntivirusProduct");
            ManagementObjectSearcher _managementObjectSearcher =
                new ManagementObjectSearcher(_managementScope, _objectQuery);
            ManagementObjectCollection _managementObjectCollection = _managementObjectSearcher.Get();
            if (_managementObjectCollection.Count > 0)
            {
                foreach (ManagementObject item in _managementObjectCollection)
                {
                    var state = int.Parse(item["productState"].ToString());
                    result += $"{item["displayName"]} ";
                    if (!State) State = state.ToString("X").Substring(1, 1).Equals("1") ? true : false;
                    result += state.ToString("X").Substring(1, 1).Equals("1") ? "Включен" : "Выключен";
                    result += "\n";
                }
            }
            result = result.Remove(result.Length-1, 1);
            return result;
        }
    }
}
