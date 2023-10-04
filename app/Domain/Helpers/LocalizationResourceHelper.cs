using Domain.Managers;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace Domain.Helpers;

public sealed class LocalizationResourceHelper
    : INotifyPropertyChanged
{
    private const string NO_FOUND_RESPONSE = "--.--";

    static readonly Lazy<LocalizationResourceHelper> currentHolder = new(() => new LocalizationResourceHelper());
    public static LocalizationResourceHelper Current => currentHolder.Value;

    CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
    public CultureInfo CurrentCulture
    {
        get => currentCulture;
        set
        {
            currentCulture = value;
            RaisePropertyChanged(nameof(CurrentCulture));
        }
    }

    public void Initialize(CultureInfo culture)
    {
        CurrentCulture = culture;
    }

    public string GetValue(string text)
    {
        try
        {
            string key = text.Split('.')[0];
            string value = text.Split('.')[1];

            ResourceManager resource = StringManager.Get(key);

            string? result = resource.GetString(value, CurrentCulture);

            if (string.IsNullOrEmpty(result))
            {
                return NO_FOUND_RESPONSE;
            }

            return result;
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message); // TODO: Change for logging.

            return NO_FOUND_RESPONSE;
        }
    }

    public string this[string text] => GetValue(text);

    #region PropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
