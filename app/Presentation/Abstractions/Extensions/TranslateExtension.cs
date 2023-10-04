using Domain.Helpers;

namespace Presentation.Abstractions.Extensions;

[ContentProperty(nameof(Text))]
public class TranslateExtension : IMarkupExtension<string>
{
    public string Text { get; set; } = string.Empty;

    public string StringFormat { get; set; } = string.Empty;

    public string ProvideValue(IServiceProvider serviceProvider)
    {
        string text = LocalizationResourceHelper.Current[Text];
        return string.IsNullOrEmpty(StringFormat)
            ? text
            : string.Format(StringFormat, text);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
