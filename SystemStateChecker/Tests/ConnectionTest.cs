using System.Net.NetworkInformation;

namespace SystemStateChecker.Tests
{
    public class ConnectionTest : ITest
    {
        private string _testUrl;
        
        public ConnectionTest(string testUrl)
        {
            _testUrl = testUrl;
        }

        public bool State { get; private set; }


        public string Result()
        {
            IPStatus state;
            try
            {
                state = new Ping().Send(_testUrl).Status;
            }
            catch
            {
                return "Error";
            }
            State=state.ToString().Equals("Success")?true:false;
            return $"Проверка соединения: {state.ToString()}";
        }
    }
}
