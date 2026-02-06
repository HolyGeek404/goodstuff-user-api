using System.Text.RegularExpressions;

namespace GoodStuff.UserApi.Domain.ValueObjects;

public sealed partial record ActivationToken
{
    private static readonly Regex Pattern = ActivationKeyRegex();
    public Guid Value { get; }
    private ActivationToken(Guid value) => Value = value;

    public static ActivationToken Create(Guid value)
    {
        return !Pattern.IsMatch(value.ToString())
            ? throw new ArgumentNullException(nameof(value))
            : new ActivationToken(value);
    }

    [GeneratedRegex("^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$",
        RegexOptions.Compiled)]
    private static partial Regex ActivationKeyRegex();
}