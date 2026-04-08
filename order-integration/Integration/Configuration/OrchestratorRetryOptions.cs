namespace Integration.Configuration
{
    public class OrchestratorRetryOptions
    {
        public int MaxNumberOfAttempts { get; set; } = 3;
        public double BackoffCoefficient { get; set; } = 2;
        public TimeSpan FirstRetryInterval { get; set; } = TimeSpan.FromSeconds(5);
        public TimeSpan MaxRetryInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}
