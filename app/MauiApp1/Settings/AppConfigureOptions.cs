using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MauiApp1.Settings;

internal sealed class AppConfigureOptions<T>
    : IConfigureOptions<T>
    where T : class
{
    private readonly IConfiguration _configuration;

    public AppConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(T options)
        => _configuration
            .GetSection(options.GetType().Name)
            .Bind(options);
}
