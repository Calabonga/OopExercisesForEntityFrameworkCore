using Calabonga.OopExercises.Domain.DomainEvents.Base;

namespace Calabonga.OopExercises.Domain.DomainEvents;

public record DocumentParticipantAddedDomainEvent(Guid DocumentId, Participant Participant) : IDomainEvent;
