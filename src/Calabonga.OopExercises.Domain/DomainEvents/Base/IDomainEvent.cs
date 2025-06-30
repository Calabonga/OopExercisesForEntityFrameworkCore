using MediatR;
using System.Text.Json.Serialization;

namespace Calabonga.OopExercises.Domain.DomainEvents.Base;

[JsonDerivedType(typeof(DocumentParticipantAddedDomainEvent))]
[JsonDerivedType(typeof(DocumentStateChangedDomainEvent))]
public interface IDomainEvent : INotification { }
