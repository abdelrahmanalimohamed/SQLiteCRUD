namespace SQLiteCRUD.CustomLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;

        public FileLoggerProvider(string filePath)
        {
            _filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_filePath);
        }
        public void Dispose()
        {
           
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (_lock)
            {
                var message = formatter(state, exception);
                if (!string.IsNullOrEmpty(message))
                {
                    File.AppendAllText(_filePath, message + Environment.NewLine);
                }

                if (exception != null)
                {
                    File.AppendAllText(_filePath, $"Exception: {exception.Message}{Environment.NewLine}");
                    File.AppendAllText(_filePath, $"Stack Trace: {exception.StackTrace}{Environment.NewLine}");
                }
            }
        }
    }
}
