using Application.Abstractions.Data;

namespace MauiApp1.Services;

internal sealed class PreferenceService : IPreferenceService
{
    public string Get(string key, string defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void Set(string key, string value)
    {
        Preferences.Set(key, value);
    }
}
