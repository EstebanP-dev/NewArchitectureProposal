using Acr.UserDialogs;
using Domain.Errors;
using Domain.Helpers;
using Presentation.Abstractions.Mvvm;
using Presentation.Abstractions.Mvvm.Input;

namespace MauiApp1.Middlewares;

internal sealed class ExceptionMiddleware : IMauiCommandMiddleWare
{
    public async Task InvokeAsync(CommandDelegate next, ViewModelBase context)
    {
		try
		{
			await next()
				.ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			context.IsBusy = false;
            Console.WriteLine(ex); // TODO: Change for logging.
			await UserDialogs.Instance.AlertAsync(message: DomainErrors.UnExceptedError.Description, title: "Error");
		}
    }
}
