using System.Text.RegularExpressions;

namespace GoodStuff.UserApi.Domain.ValueObjects;

public sealed partial record Password
{
    private static readonly Regex Pattern = PasswordRegex();
    public string Value { get; }
    private Password(string value) => Value = value;

    public static Password Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
            throw new ArgumentNullException(nameof(value));
        
        var valueTrim = value.Trim();
        return new Password(valueTrim);
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$", RegexOptions.Compiled)]
    private static partial Regex PasswordRegex();
}
