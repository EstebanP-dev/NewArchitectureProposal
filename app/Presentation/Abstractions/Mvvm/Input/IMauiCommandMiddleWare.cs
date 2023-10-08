using Presentation.Abstractions.Dependencies;

namespace Presentation.Abstractions.Mvvm.Input;

public delegate Task CommandDelegate();

public interface IMauiCommandMiddleWare : ITransientDependency
{
    Task InvokeAsync(CommandDelegate next, ViewModelBase context);
}
