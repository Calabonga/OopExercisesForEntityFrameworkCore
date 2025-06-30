using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.Base;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace Calabonga.OopExercises.Infrastructure.Interceptors;

public class ConvertDomainEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        HandleSavingChanges(eventData);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        HandleSavingChanges(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleSavingChanges(DbContextEventData eventData)
    {
        var roots = eventData.Context!.ChangeTracker.Entries<AggregateRoot>();
        var many = roots.Select(x => x.Entity).SelectMany(x =>
        {
            var domainEvents = x.GetDomainEvents();
            x.ClearDomainEvents();
            return domainEvents;
        }).ToList();

        var outboxMessages = many.Select(x => new OutboxMessage
        {
            Id = Guid.Empty,
            Type = x.GetType().Name,
            CreatedAtUtc = DateTime.UtcNow,
            Content = JsonSerializer.Serialize(x)
        }).ToList();

        eventData.Context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
