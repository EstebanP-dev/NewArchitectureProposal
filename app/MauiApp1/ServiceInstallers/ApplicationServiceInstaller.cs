using Application.Abstractions.Behaviors;
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
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

        services
            .AddValidatorsFromAssembly(
                assembly: Application.AssemblyReference.Assembly,
                includeInternalTypes: true);

        services
            .ConfigureOptions<AppConfigureOptions<ApiSettings>>();

    }
}
