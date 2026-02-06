using System.Text.RegularExpressions;

namespace GoodStuff.UserApi.Domain.ValueObjects;

public sealed partial record Name
{
    public string Value { get; }
    private static readonly Regex Pattern = NameRegex();

    private Name(string value)
    {
        Value = value;
    }

    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
            throw new ArgumentNullException(nameof(value));

        var valueTrim = value.Trim();

        return new Name(valueTrim);
    }

    [GeneratedRegex(@"^[A-Za-z]+(?:[ -][A-Za-z]+)*$", RegexOptions.Compiled)]
    private static partial Regex NameRegex();
}