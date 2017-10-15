namespace SystemStateChecker.Tests
{
    interface ITest
    {
        bool State { get; }
        string Result();
    }
}
