using Calabonga.OopExercises.Domain.Base;

namespace Calabonga.OopExercises.Domain;

public abstract class Auditable : Entity
{
    protected Auditable(Guid id) : base(id)
    {

    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string UpdatedBy { get; set; }
}
