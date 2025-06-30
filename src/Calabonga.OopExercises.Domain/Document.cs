using Calabonga.OopExercises.Domain.Base;
using Calabonga.OopExercises.Domain.DomainEvents;
using Calabonga.OopExercises.Domain.ValueObjects;

namespace Calabonga.OopExercises.Domain;

public class Document : AggregateRoot
{
    private readonly List<Participant> _participants = new();
    private readonly List<DocumentState> _stateHistory = new();

    private Document(Guid id, TitleValue title) : base(id)
    {
        Title = title;
    }

    public static Document Create(Guid id, string title)
    {
        var titleValue = TitleValue.Create(title).Result;
        return new Document(id, titleValue);
    }

    public string? Description { get; private set; }

    public TitleValue Title { get; }

    public DocumentState State => StateHistory.Last();

    public IReadOnlyCollection<DocumentState> StateHistory => _stateHistory.AsReadOnly();

    public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

    public void AddDescription(string? description)
    {
        Description = description;
    }
    public void AddState(DocumentState state, Participant participant)
    {
        SetParticipant(participant);
        _stateHistory.Add(state);
        RaiseDomainEvent(new DocumentStateChangedDomainEvent(Id, state));
    }

    private void SetParticipant(Participant participant)
    {
        var exists = _participants.Find(x => x.Id == participant.Id);
        if (exists is not null)
        {
            return;
        }

        _participants.Add(participant);
        RaiseDomainEvent(new DocumentParticipantAddedDomainEvent(Id, participant));
    }
}
