using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presentation.Abstractions.Mvvm.Input;

public sealed class CustomAsyncCommand : Command
{
    public CustomAsyncCommand(Action<object> execute) : base(execute)
    {
    }

    public CustomAsyncCommand(Action execute) : base(execute)
    {
    }

    public CustomAsyncCommand(ViewModelBase context, IServiceProvider provider, Func<Task> execute)
        : base(ExecuteAsync(provider, execute, context))
    {
    }
    
    public CustomAsyncCommand(ViewModelBase context, IServiceProvider provider, Func<object, Task> execute)
        : base(ExecuteAsync(provider, execute, context))
    {
    }

    public CustomAsyncCommand(Action<object> execute, Func<object, bool> canExecute) : base(execute, canExecute)
    {
    }

    public CustomAsyncCommand(Action execute, Func<bool> canExecute) : base(execute, canExecute)
    {
    }

    private static Action ExecuteAsync(IServiceProvider provider, Func<Task> execute, ViewModelBase context) => async () =>
    {
        List<Task> invokes = new();
        IEnumerable<IMauiCommandMiddleWare> _middlewares = provider.GetServices<IMauiCommandMiddleWare>();

        async Task CommandDelegate() => await execute();

        foreach (IMauiCommandMiddleWare middleware in _middlewares)
        {
            invokes.Add(middleware.InvokeAsync(CommandDelegate, context));
        }

        await Task.WhenAll(invokes);
    };
    
    private static Action<object> ExecuteAsync(IServiceProvider provider, Func<object, Task> execute, ViewModelBase context) => async (paramater) =>
    {
        IEnumerable<IMauiCommandMiddleWare> _middlewares = provider.GetServices<IMauiCommandMiddleWare>();

        CommandDelegate commandDelegate = async () => await execute(paramater);

        foreach (IMauiCommandMiddleWare middleware in _middlewares)
        {
            commandDelegate = async () => await middleware.InvokeAsync(commandDelegate, context);
        }

        await commandDelegate();
    };
}
