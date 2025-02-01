using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace Librarian.Converters;

public class InverseBooleanConverter : BaseConverterOneWay<bool?, object?>
{
    [return: NotNullIfNotNull(nameof(value))]
    public override object? ConvertFrom(bool? value, CultureInfo? culture)
    {
        return !value;
    }

    public override object? DefaultConvertReturnValue { get; set; } = null;
}