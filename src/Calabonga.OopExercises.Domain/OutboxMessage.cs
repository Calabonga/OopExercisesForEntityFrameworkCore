namespace Calabonga.OopExercises.Domain;

public class OutboxMessage
{
    public Guid Id { get; set; }

    public required string Type { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ProcessedAtUtc { get; set; }

    public string? Error { get; set; }
}
