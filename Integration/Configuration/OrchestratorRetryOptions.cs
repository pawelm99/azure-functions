namespace Integration.Configuration
{
    public class OrchestratorRetryOptions
    {
        public int MaxNumberOfAttempts { get; set; } = 1;
        public double BackoffCoefficient { get; set; } = 1;
        public TimeSpan FirstRetryInterval { get; set; } = TimeSpan.FromMinutes(31);
        public TimeSpan MaxRetryInterval { get; set; } = TimeSpan.FromMinutes(31);
    }
}
