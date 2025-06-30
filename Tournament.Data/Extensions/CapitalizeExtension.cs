namespace Tournament.Data.Extensions;

public static class CapitalizeExtension
{
    public static string Capitalize(this string value) => 
        value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            "" => value,
            _ => string.Concat(value[0].ToString().ToUpperInvariant(), value.Substring(1))
        };
}
