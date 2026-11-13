using System.Text.RegularExpressions;
using HMS.Domain.Common;

namespace HMS.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    private static readonly Regex EgyptianPhoneRegex =
        new(@"^01[0125][0-9]{8}$", RegexOptions.Compiled);

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberErrors.Empty);
        }

        var trimmed = value.Trim();

        if (!EgyptianPhoneRegex.IsMatch(trimmed))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberErrors.InvalidFormat);
        }

        return Result.Success(new PhoneNumber(trimmed));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
