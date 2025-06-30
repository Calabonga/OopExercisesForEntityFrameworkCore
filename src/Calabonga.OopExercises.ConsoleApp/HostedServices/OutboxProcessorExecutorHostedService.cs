using Calabonga.Microservices.BackgroundWorkers;
using Calabonga.OopExercises.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.OopExercises.ConsoleApp.HostedServices;

public class OutboxProcessorExecutorHostedService : ScheduledHostedServiceBase
{
    public OutboxProcessorExecutorHostedService(IServiceScopeFactory serviceScopeFactory, ILogger logger)
        : base(serviceScopeFactory, logger)
    {
    }

    protected override async Task ProcessInScopeAsync(IServiceProvider serviceProvider, CancellationToken token)
    {
        var outboxProcessor = serviceProvider.GetRequiredService<IOutboxProcessor>();
        await outboxProcessor.ProcessAsync(serviceProvider, token);
    }

    protected override string Schedule => "0/15 * * * * *"; // every 15 seconds

    protected override bool IncludingSeconds => true;

    protected override string DisplayName => "OutboxProcessorExecutorHostedService";

#if DEBUG
    protected override bool IsExecuteOnServerRestart => true;
#else
    protected override bool IsExecuteOnServerRestart => false;
#endif
}
