using Microsoft.DurableTask;

namespace Integration.Configuration
{
    public interface ITaskOptionsProvider
    {
        TaskOptions GetDefaultTaskOptions();
    }
}
