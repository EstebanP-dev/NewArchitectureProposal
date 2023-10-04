using Application.Abstractions.Data;
using Application.Models.Settings;
using CommunityToolkit.Maui;
using MauiApp1.DelegatingHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;
using System.Reflection;

namespace MauiApp1.ServiceInstallers;

internal sealed class WebApiServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Scan(scan => scan.FromAssemblies(AssemblyReference.Assembly)
                .AddClasses(x => x.AssignableTo<DelegatingHandler>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

        services.AddHttpClient(nameof(ApiSettings.General), (provider, httpClient) =>
        {
            IPreferenceService preferenceService = provider.GetService<IPreferenceService>();
            ApiSettings settings = provider.GetService<IOptions<ApiSettings>>().Value;

            httpClient.BaseAddress = new Uri(settings.BaseUrl + settings.Version);
        })
        .AddHttpMessageHandler<LoggingHandler>()
        .AddHttpMessageHandler<GeneralAuthenticationHandler>()
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            };
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
    }
}
