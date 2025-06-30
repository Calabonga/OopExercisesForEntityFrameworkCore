namespace Calabonga.OopExercises.Infrastructure;

public interface IOutboxProcessor
{
    Task ProcessAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
}
