namespace Application.Abstractions.Data;

public interface IPreferenceService
{
    void Set(string key, string value);
    string Get(string key, string defaultValue);
}
