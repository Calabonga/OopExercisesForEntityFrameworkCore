using Calabonga.OopExercises.Domain.ValueObjects;
using Calabonga.OperationResults;

namespace Calabonga.OopExercises.Domain;

public class DocumentState : Auditable
{
    private const int MaxTitleLength = 128;
    private const int MaxParticipantNameLength = 128;

    private DocumentState(Guid id, TitleValue title, string createdBy)
        : base(id)
    {
        Title = title;
        CreatedBy = createdBy;
    }

    public TitleValue Title { get; }

    public Guid DocumentId { get; set; }

    public static Operation<DocumentState, string> Create(Guid id, string title, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Operation.Error("Value is empty or null");
        }

        if (string.IsNullOrWhiteSpace(createdBy))
        {
            return Operation.Error("CreatedBy name is empty or null");
        }

        if (title.Length > MaxTitleLength)
        {
            return Operation.Error("Value is greater than max length");
        }

        if (createdBy.Length > MaxParticipantNameLength)
        {
            return Operation.Error("Participant is greater than max length");
        }

        var titleValue = TitleValue.Create(title);
        if (titleValue.Ok)
        {
            return new DocumentState(id, titleValue.Result, createdBy);
        }

        return Operation.Error(titleValue.Error);
    }
}
