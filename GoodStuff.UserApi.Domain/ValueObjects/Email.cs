using System.Text.RegularExpressions;

namespace GoodStuff.UserApi.Domain.ValueObjects;

public sealed partial record Email
{
    private static readonly Regex Pattern = EmailRegex();
    public string Value { get; }
    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
            throw new ArgumentNullException(nameof(value));

        var valueTrim = value.Trim();
        return new Email(valueTrim);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}