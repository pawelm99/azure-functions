using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Integration.Providers
{
    public class BlobLoggerProvider : ILoggerProvider
    {
        private readonly BlobContainerClient _container;


        public BlobLoggerProvider(string connectionString, string containerName)
        {
            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists();
        }


        public ILogger CreateLogger(string categoryName) => new BlobLogger(_container, categoryName);


        public void Dispose() { }


        private class BlobLogger : ILogger
        {
            private readonly BlobContainerClient _container;
            private readonly string _category;
            private static readonly object _lock = new();

            public BlobLogger(BlobContainerClient container, string category)
            {
                _container = container;
                _category = category;
            }


            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
            public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;


            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel)) return;

                string message = $"[{DateTime.UtcNow:O}] [{_category}] [{logLevel}]: {formatter(state, exception)}{Environment.NewLine}";
                if (exception != null) message += $"Exception: {exception}{Environment.NewLine}";

                var blobName = $"logs/{DateTime.UtcNow:yyyy-MM-dd}.log";
                lock (_lock)
                {
                    var appendBlobClient = _container.GetAppendBlobClient(blobName);

                    if (!appendBlobClient.Exists())
                    {
                        appendBlobClient.Create();
                    }

                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
                    appendBlobClient.AppendBlock(stream);
                }
            }
        }
    }
}
