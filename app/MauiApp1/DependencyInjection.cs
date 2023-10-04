using MauiApp1.ServiceInstallers;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MauiApp1;

internal static class DependencyInjection
{
    private const string ENVIROMENT = "QA";

    internal static MauiAppBuilder Initialize(this MauiAppBuilder builder)
    {
        builder.Configuration.AddConfiguration();

        AddServiceInstallers(builder.Services, builder.Configuration);

        return builder;
    }

    private static void AddServiceInstallers(IServiceCollection services, IConfiguration configuration)
    {
        IEnumerable<IServiceInstaller> serviceInstallers = Assembly
            .GetExecutingAssembly()
            .DefinedTypes
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (IServiceInstaller serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }
    }

    private static bool IsAssignableToType<T>(TypeInfo typeInfo)
    {
        return typeof(T)
            .IsAssignableFrom(typeInfo)
            && !typeInfo.IsInterface
            && !typeInfo.IsAbstract;
    }

    private static void AddConfiguration(this ConfigurationManager configurationManager)
    {
        string settingPath = ENVIROMENT == "QA" ? "MauiApp1.appsettings.QA.json" : "MauiApp1.appsettings.PROD.json";

        Assembly assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(settingPath);
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        configurationManager.AddConfiguration(configuration);
    }
}
