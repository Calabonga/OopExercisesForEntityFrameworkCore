using Calabonga.OopExercises.ConsoleApp.HostedServices;
using Calabonga.OopExercises.Infrastructure;
using Calabonga.OopExercises.Infrastructure.Interceptors;
using Calabonga.UnitOfWork;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Calabonga.OopExercises.ConsoleApp;

/// <summary>
/// Create Container for Console App
/// </summary>
public static class ConsoleApp
{
    /// <summary>
    /// Creates container <see cref="ServiceCollection"/>
    /// </summary>
    /// <returns></returns>
    public static ServiceProvider CreateContainer(Action<IServiceCollection>? additionalServices = null)
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: false)
            .AddDotNetEnv(".env", LoadOptions.TraversePath())
            .Build();

        var logger = new LoggerConfiguration().MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(x => x.AddSerilog(logger));

        services.Configure<AppSettings>(x => configuration.GetSection(nameof(AppSettings)).Bind(x));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(nameof(ApplicationDbContext)),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .AddInterceptors(
                    serviceProvider.GetRequiredService<AuditableDataInterceptor>(),
                    serviceProvider.GetRequiredService<ConvertDomainEventToOutboxMessageInterceptor>());
        });

        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<AuditableDataInterceptor>();
        services.AddScoped<ConvertDomainEventToOutboxMessageInterceptor>();

        services.AddHostedService<OutboxProcessorExecutorHostedService>();

        additionalServices?.Invoke(services);


        return services.BuildServiceProvider();
    }
}
