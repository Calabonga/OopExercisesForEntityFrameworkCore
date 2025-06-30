using MediatR;
using Microsoft.Extensions.Logging;

namespace Calabonga.OopExercises.Domain.DomainEvents.Handlers;

public class DocumentStateChangedDomainEventHandler : INotificationHandler<DocumentStateChangedDomainEvent>
{
    private readonly ILogger<DocumentStateChangedDomainEventHandler> _logger;

    public DocumentStateChangedDomainEventHandler(ILogger<DocumentStateChangedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DocumentStateChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} handled.", GetType().Name);

        return Task.CompletedTask;
    }
}
