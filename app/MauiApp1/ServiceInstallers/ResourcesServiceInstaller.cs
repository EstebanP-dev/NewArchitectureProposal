using Domain.Helpers;
using Domain.Managers;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace MauiApp1.ServiceInstallers;

internal sealed class ResourcesServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        LocalizationResourceHelper.Current.Initialize(new CultureInfo("en-US"));

        Domain.AssemblyReference.LoadResourceManagers();
        Presentation.AssemblyReference.LoadResourceManagers();
    }
}
