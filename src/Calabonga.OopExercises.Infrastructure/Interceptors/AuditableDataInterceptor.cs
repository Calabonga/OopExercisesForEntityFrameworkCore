using Calabonga.OopExercises.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Calabonga.OopExercises.Infrastructure.Interceptors;

public class AuditableDataInterceptor : SaveChangesInterceptor
{
    private const string DefaultUserName = "Anonymous";

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

    private static void HandleSavingChanges(DbContextEventData eventData)
    {
        var context = eventData.Context;
        if (context is null)
        {
            return;
        }

        var createdEntries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        foreach (var entry in createdEntries)
        {
            if (!(entry.Entity is Auditable))
            {
                continue;
            }

            var creationDate = DateTime.Now.ToUniversalTime();
            var userName = entry.Property("CreatedBy").CurrentValue == null
                ? DefaultUserName
                : entry.Property("CreatedBy").CurrentValue;
            var updatedAt = entry.Property("UpdatedAt").CurrentValue;
            var createdAt = entry.Property("CreatedAt").CurrentValue;
            if (createdAt != null)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.Parse(createdAt.ToString() ?? string.Empty).Year > 1970
                    ? ((DateTime)createdAt).ToUniversalTime()
                    : creationDate;
            }
            else
            {
                entry.Property("CreatedAt").CurrentValue = creationDate;
            }

            if (updatedAt != null)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.Parse(updatedAt.ToString() ?? string.Empty).Year > 1970
                    ? ((DateTime)updatedAt).ToUniversalTime()
                    : creationDate;
            }
            else
            {
                entry.Property("UpdatedAt").CurrentValue = creationDate;
            }

            entry.Property("CreatedBy").CurrentValue = userName;
            entry.Property("UpdatedBy").CurrentValue = userName;
        }

        var updatedEntries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
        foreach (var entry in updatedEntries)
        {
            if (entry.Entity is Auditable)
            {
                var userName = entry.Property("UpdatedBy").CurrentValue == null
                    ? DefaultUserName
                    : entry.Property("UpdatedBy").CurrentValue;
                entry.Property("UpdatedAt").CurrentValue = DateTime.Now.ToUniversalTime();
                entry.Property("UpdatedBy").CurrentValue = userName;
            }

        }
    }
}
