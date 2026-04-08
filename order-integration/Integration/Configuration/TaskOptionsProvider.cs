using Microsoft.DurableTask;
using Microsoft.Extensions.Options;

namespace Integration.Configuration
{
    public class TaskOptionsProvider : ITaskOptionsProvider
    {
        private readonly OrchestratorRetryOptions _options;


        public TaskOptionsProvider(IOptions<OrchestratorRetryOptions> options)
        {
            _options = options.Value;
        }


        public TaskOptions GetDefaultTaskOptions()
        {
            var retryPolicy = new RetryPolicy(
                maxNumberOfAttempts: _options.MaxNumberOfAttempts,
                backoffCoefficient: _options.BackoffCoefficient,
                firstRetryInterval: _options.FirstRetryInterval,
                maxRetryInterval: _options.MaxRetryInterval);

            return new TaskOptions
            {
                Retry = retryPolicy
            };
        }
    }
}
