using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace SystemStateChecker.Tests
{
    public class AntivirusTest : ITest
    {
        private bool _state;
        private List<bool> states;
        private ConnectionOptions _connectionOptions;
        private ManagementScope _managementScope;
        private ObjectQuery _objectQuery;
        private ManagementObjectSearcher _managementObjectSearcher;
        private ManagementObjectCollection _managementObjectCollection;

        public bool State
        {
            get
            {
                _managementObjectCollection = _managementObjectSearcher.Get();
                states = new List<bool>();
                foreach (ManagementObject item in _managementObjectCollection)
                {
                    var state = int.Parse(item["productState"].ToString());
                    states.Add(state.ToString("X").Substring(1, 1).Equals("1") ? true : false);
                }
                return states.Any(b => b.Equals(true));
            }
        }

        public AntivirusTest()
        {
            _state = false;
            _connectionOptions = new ConnectionOptions { EnablePrivileges = true, Impersonation = ImpersonationLevel.Impersonate };
            _managementScope = new ManagementScope($"\\\\localhost\\root\\SecurityCenter2", _connectionOptions);
            _managementScope.Connect();
            _objectQuery = new ObjectQuery("SELECT * FROM AntivirusProduct");
            _managementObjectSearcher =
                new ManagementObjectSearcher(_managementScope, _objectQuery);
        }

        public string CurrentAv()
        {
            string result = string.Empty;
            _managementObjectCollection = _managementObjectSearcher.Get();
            if (_managementObjectCollection.Count > 0)
            {
                foreach (ManagementObject item in _managementObjectCollection)
                {
                    var state = int.Parse(item["productState"].ToString());
                    if (state.ToString("X").Substring(1, 1).Equals("1")) return $"{item["displayName"]} ";
                }
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        public string Result()
        {
            string result = string.Empty;
            _managementObjectCollection = _managementObjectSearcher.Get();
            if (_managementObjectCollection.Count > 0)
            {
                foreach (ManagementObject item in _managementObjectCollection)
                {
                    var state = int.Parse(item["productState"].ToString());
                    result += $"{item["displayName"]} ";
                    if (!_state) _state = state.ToString("X").Substring(1, 1).Equals("1") ? true : false;
                    result +=_state ? "Включен" : "Выключен";
                    result += "\n";
                }
            }
            result = result.Remove(result.Length-1, 1);
            return result;
        }
    }
}
