using Calabonga.OopExercises.Domain.ValueObjects;

namespace Calabonga.OopExercises.Domain;

public class Participant : Auditable
{
    private Participant(Guid id, string firstName, string lastName, PositionValue position)
        : base(id)
    {
        Position = position;
        FirstName = firstName;
        LastName = lastName;
    }

    public PositionValue Position { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public Guid DocumentId { get; set; }

    public static Participant Create(string firstName, string lastName, PositionValue position)
    {
        return new Participant(Guid.Empty, firstName, lastName, position);
    }
}
