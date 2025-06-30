using Calabonga.OopExercises.Domain.DomainEvents.Base;

namespace Calabonga.OopExercises.Domain.DomainEvents;

public record DocumentStateChangedDomainEvent(Guid DocumentId, DocumentState State) : IDomainEvent;
