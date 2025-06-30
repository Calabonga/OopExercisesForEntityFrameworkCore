using Calabonga.OopExercises.Domain.Base;
using Calabonga.OperationResults;

namespace Calabonga.OopExercises.Domain.ValueObjects;
public class PositionValue : ValueObject
{
    public const int MaxLength = 256;

    private PositionValue(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Operation<PositionValue, string> Create(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
        {
            return Operation.Error("Value is empty");
        }

        if (position.Length > MaxLength)
        {
            return Operation.Error("Value is greater than Max value");
        }

        return new PositionValue(position);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
