using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.DomainEvents.Base;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace Calabonga.OopExercises.Infrastructure;

public class OutboxProcessor : IOutboxProcessor
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly TimeProvider _timerProvider;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        TimeProvider timerProvider,
        ILogger<OutboxProcessor> logger)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _timerProvider = timerProvider;
        _logger = logger;
    }
    public async Task ProcessAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var pagedList = await _unitOfWork.GetRepository<OutboxMessage>()
            .GetPagedListAsync(
                predicate: x => x.ProcessedAtUtc == null,
                pageIndex: 0,
                pageSize: 20,
                orderBy: o => o.OrderBy(x => x.CreatedAtUtc),
                trackingType: TrackingType.Tracking,
                cancellationToken: cancellationToken
            );

        foreach (var message in pagedList.Items)
        {
            var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(message.Content);
            if (domainEvent is null)
            {
                _logger.LogCritical(
                    "DomainEvent {Type} created at {CreatedAt} does not contains data.",
                    message.Type,
                    message.CreatedAtUtc);

                continue;
            }

            await _publisher.Publish(domainEvent, cancellationToken);

            message.ProcessedAtUtc = _timerProvider.GetUtcNow().DateTime;

            await _unitOfWork.SaveChangesAsync();

            if (!_unitOfWork.Result.Ok)
            {
                var error = _unitOfWork.Result.Exception ?? new DataException("Something went wrong!");
                _logger.LogError(error, error.Message);
                return;
            }

            _logger.LogInformation("Process completed.");
        }
    }
}
