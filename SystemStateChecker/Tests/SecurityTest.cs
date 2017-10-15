using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SystemStateChecker.Tests
{
    public class SecurityTest : ITest
    {
        public bool State
        {
            get => !File.Exists("test.txt");
            private set => State = value;
        }
        public string Result()
        {
            using (FileStream fs = File.Create("test.txt"))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");
                }
            }
            Thread.Sleep(1000);
            return !File.Exists("test.txt") ? "Ваш антивирус успешно справился с угрозой" : "Ваш антивирус не работает";
        }

        public Task<string> ResultAsync()
        {
            return Task.Run(() => Result());
        }

    }
}
