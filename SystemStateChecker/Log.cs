using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStateChecker
{
    public class Log
    {
        private StringBuilder _log;

        public bool connectionTestStateChanged { get; set; }
        public bool firewallTestStateChanged { get; set; }
        public bool antivirusTestStateChanged { get; set; }
        public bool securityTestStateChanged { get; set; }
    }
}
