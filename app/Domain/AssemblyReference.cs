using Domain.Errors;
using Domain.Features.Auth.Errors;
using Domain.Managers;
using System.Reflection;

namespace Domain;

public static class AssemblyReference
{
    public static Assembly Assembly { get; } =  typeof(AssemblyReference).Assembly;

    public static void LoadResourceManagers()
    {
        StringManager.Add(nameof(DomainErrorResource), DomainErrorResource.ResourceManager);
        StringManager.Add(nameof(AuthErrorsResource), AuthErrorsResource.ResourceManager);
    }
}
