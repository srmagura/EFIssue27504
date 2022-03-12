using ITI.DDD.Logging;

namespace Logging
{
    public class BugsnagLogWriter : ILogWriter
    {
        private readonly DbLogWriter _dbLogWriter;
        private readonly Bugsnag.IClient _bugsnag;

        public BugsnagLogWriter(DbLogWriter dbLogWriter, Bugsnag.IClient bugsnag)
        {
            _dbLogWriter = dbLogWriter;
            _bugsnag = bugsnag;
        }

        public void Write(
            string level,
            string? userId,
            string? userName,
            string hostname,
            string process,
            string message,
            Exception? exception
        )
        {
            _dbLogWriter.Write(level, userId, userName, hostname, process, message, exception);

            if (level.Equals("error", StringComparison.OrdinalIgnoreCase) || exception != null)
            {
                var bugsnagException = exception ?? new Exception(message);

                _bugsnag.Notify(bugsnagException, report =>
                {
                    report.Event.User = new Bugsnag.Payload.User
                    {
                        Id = userId,
                        Name = userName
                    };
                });
            }
        }
    }
}
