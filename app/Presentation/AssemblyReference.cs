using Domain.Managers;
using Presentation.Features.Auth.Login;
using System.Reflection;

namespace Presentation;

public static class AssemblyReference
{
    public static Assembly Assembly { get; } =  typeof(AssemblyReference).Assembly;

    public static void LoadResourceManagers()
    {
        StringManager.Add(nameof(LoginResource), LoginResource.ResourceManager);
    }
}
