using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStateChecker.Tests
{
    interface ITest
    {
        bool State { get; }
        string Result();
    }
}
