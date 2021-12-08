using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScopedLoggingDemo;

public class Function1
{
    private const string FunctionName = "FunctionWithScopedLogging";
    private const string FunctionNameErrors = "FunctionWithScopedLogging_Errors";
    private const string FunctionNameNested = "FunctionWithScopedLogging_Nested";



    private ILogger<Function1> _logger;
    private ISomeService _someService;

    public Function1(ISomeService someService, ILogger<Function1> logger)
    {
        _someService = someService;
        _logger = logger;
    }


    [FunctionName(FunctionName)]
    public async Task<IActionResult> FunctionWithScopedLogging([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object> { { "ScopeName", FunctionName }, { "ScopeBeginTime", DateTime.UtcNow }, { "ScopeId", Guid.NewGuid() } });

        _logger.LogInformation("Log from within Function Run");
        _logger.LogInformation("Structured log from within function run at {currentTime}", DateTime.UtcNow);


        _logger.LogInformation("Calling SomeService to DoSomeWork");
        await _someService.DoSomeWork(10);
        _logger.LogInformation("DoSomeWork completed successfully");



        return new OkObjectResult($"Success, completed at {DateTime.UtcNow:o}");
    }

    [FunctionName(FunctionNameErrors)]
    public async Task<IActionResult> FunctionWithScopedLoggingAndErrors([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object> { { "ScopeName", FunctionNameErrors }, { "ScopeBeginTime", DateTime.UtcNow }, { "ScopeId", Guid.NewGuid() } });

        _logger.LogInformation("Log from within Function Run");
        _logger.LogInformation("Structured log from within function run at {currentTime}", DateTime.UtcNow);


        _logger.LogInformation("Calling SomeService to DoSomeWork");
        await _someService.DoSomeWorkAndError();
        _logger.LogInformation("DoSomeWork completed successfully");



        return new OkObjectResult($"Success, completed at {DateTime.UtcNow:o}");
    }

    [FunctionName(FunctionNameNested)]
    public async Task<IActionResult> FunctionWithNestedScopes([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object> { { "ScopeName", FunctionNameNested }, { "ScopeBeginTime", DateTime.UtcNow }, { "ScopeId", Guid.NewGuid() } });

        _logger.LogInformation("Log from within Function Run");
        _logger.LogInformation("Structured log from within function run at {currentTime}", DateTime.UtcNow);


        _logger.LogInformation("Calling SomeService to DoSomeWork");
        await _someService.DoSomeWorkScopedLogs(10);
        _logger.LogInformation("DoSomeWork completed successfully");



        return new OkObjectResult($"Success, completed at {DateTime.UtcNow:o}");
    }
}