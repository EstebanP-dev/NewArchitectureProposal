using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MauiApp1.ServiceInstallers;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
