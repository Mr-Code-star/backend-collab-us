using System.Text.RegularExpressions;
namespace backend_collab_us.Shared.Domain.Infrastructure.Interfaces.ASP.Configuration.Extentions;

public static partial class StringExtensions
{
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return KebabCaseRegex().Replace(text, "-$1")
            .Trim()
            .ToLower();
    }
    
    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled)]
    private static partial Regex KebabCaseRegex();
}