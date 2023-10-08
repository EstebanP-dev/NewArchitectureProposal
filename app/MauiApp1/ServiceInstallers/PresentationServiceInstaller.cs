using Microsoft.Extensions.Configuration;
using Presentation.Abstractions.Dependencies;
using Presentation.Abstractions.Pages;
using Scrutor;
using System.Reflection;

namespace MauiApp1.ServiceInstallers;

internal sealed class PresentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Scan(scan => scan.FromAssemblies(
                Presentation.AssemblyReference.Assembly,
                AssemblyReference.Assembly)
                .AddClasses(x => x.AssignableTo<ITransientDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
        
        services
            .Scan(scan => scan.FromAssemblies(
                Presentation.AssemblyReference.Assembly,
                AssemblyReference.Assembly)
                .AddClasses(x => x.AssignableTo<IScopeDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        
        services
            .Scan(scan => scan.FromAssemblies(
                Presentation.AssemblyReference.Assembly,
                AssemblyReference.Assembly)
                .AddClasses(x => x.AssignableTo<ISingletonDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());

        List<TypeInfo> pagesInfo = Presentation.AssemblyReference.Assembly
            .DefinedTypes
            .Where(type => IsAssignableToType<IPageBase>(type.GetTypeInfo()))
            .ToList();

        if (pagesInfo.Any())
        {
            pagesInfo.ForEach((info) =>
            {
                string route = info.Name;
                Routing.RegisterRoute(route, info);
            });
        }
    }

    private static bool IsAssignableToType<T>(TypeInfo typeInfo)
    {
        return typeof(T)
            .IsAssignableFrom(typeInfo)
            && !typeInfo.IsInterface
            && !typeInfo.IsAbstract;
    }
}
