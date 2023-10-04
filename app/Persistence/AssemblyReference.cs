using System.Reflection;

namespace Persistence;

public static class AssemblyReference
{
    public static Assembly Assembly { get; } =  typeof(AssemblyReference).Assembly;
}
