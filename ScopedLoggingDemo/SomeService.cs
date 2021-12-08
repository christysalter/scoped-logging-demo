using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScopedLoggingDemo;

public interface ISomeService
{
    Task DoSomeWork(int count);
    Task DoSomeWorkScopedLogs(int count);
}

public class SomeService : ISomeService
{
    private readonly ILogger<SomeService> _logger;
    public SomeService(ILogger<SomeService> logger)
    {
        _logger = logger;
    }


    public async Task DoSomeWork(int count)
    {
        _logger.LogInformation("Log from within service");
        _logger.LogInformation("About to run {count} times", count);

        for (int i = 0; i < count; i++)
        {
            await Task.Delay(100);
            _logger.LogInformation("Completed iteration {i}", i + 1);
        }

        _logger.LogInformation("Completed DoSomeWork");
    }


    public async Task DoSomeWorkScopedLogs(int count)
    {
        using var scope = _logger.BeginLogScope(nameof(SomeService));

        _logger.LogInformation("Log from within service");
        _logger.LogInformation("About to run {count} times", count);

        for (int i = 0; i < count; i++)
        {
            await Task.Delay(100);
            _logger.LogInformation("Completed iteration {i}", i + 1);
        }

        _logger.LogInformation("Completed DoSomeWork");
    }

}
