namespace Filters.Extensions;

public static class StringExtensions
{
    internal static string ToCamelCase(this string s)
    {
        return $"{char.ToLowerInvariant(s[0])}{s.Substring(1)}";
    }
}