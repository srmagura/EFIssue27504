namespace TestDataLoader
{
    public class TestDataMonitor
    {
        private DateTimeOffset _lastStepCompletedUtc = DateTimeOffset.UtcNow;

        public void WriteCompletedMessage(string message)
        {
            var diff = DateTimeOffset.UtcNow - _lastStepCompletedUtc;
            Console.WriteLine($"[{diff.TotalSeconds:F3} s]    {message}");

            _lastStepCompletedUtc = DateTimeOffset.UtcNow;
        }

        public void WriteMessage(string message)
        {
            Console.WriteLine("             " + message);
        }
    }
}
