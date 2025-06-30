using MediatR;
using Microsoft.Extensions.Logging;

namespace Calabonga.OopExercises.Domain.DomainEvents.Handlers;

public class DocumentParticipantAddedDomainEventHandler : INotificationHandler<DocumentParticipantAddedDomainEvent>
{
    private readonly ILogger<DocumentParticipantAddedDomainEventHandler> _logger;

    public DocumentParticipantAddedDomainEventHandler(ILogger<DocumentParticipantAddedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DocumentParticipantAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} handled.", GetType().Name);

        return Task.CompletedTask;
    }
}
