using Application.Models.Settings;
using FluentValidation;
using MauiApp1.Settings;
using Microsoft.Extensions.Configuration;

namespace MauiApp1.ServiceInstallers;

internal sealed class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(config =>
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));

        services
            .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);

        services
            .ConfigureOptions<AppConfigureOptions<ApiSettings>>();

    }
}
