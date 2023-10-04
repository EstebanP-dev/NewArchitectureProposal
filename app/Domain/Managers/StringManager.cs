using System.Resources;

namespace Domain.Managers;

public static class StringManager
{
    private static readonly Dictionary<string, ResourceManager> _dictionary = new();

    public static void Add(string key, ResourceManager resource)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        ArgumentNullException.ThrowIfNull(resource, nameof(resource));

        if (!_dictionary.TryAdd(key, resource))
        {
            throw new KeyAlreadyAddedException();
        }
    }

    public static void Remove(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        if (!_dictionary.Remove(key))
        {
            throw new KeyNotFoundException();
        }
    }

    public static void Clear() => _dictionary.Clear();

    public static ResourceManager Get(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        if (!_dictionary.TryGetValue(key, out ResourceManager? resource))
        {
            throw new KeyNotFoundException();
        }

        ArgumentNullException.ThrowIfNull(resource, nameof(resource));

        return resource;
    }
}
