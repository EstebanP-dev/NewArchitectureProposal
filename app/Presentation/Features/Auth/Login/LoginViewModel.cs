using Acr.UserDialogs;
using Application.Features.Auth;
using CommunityToolkit.Mvvm.Input;
using Presentation.Abstractions.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace Presentation.Features.Auth.Login;

public sealed partial class LoginViewModel : ViewModelBase
{
    private readonly IMediator _sender;

    [ObservableProperty]
    EmailValidator emailValidator;

    [ObservableProperty]
    public string password;

    public ICommand HandleSubmitCommand { get; }

    public LoginViewModel(IMediator sender, IServiceProvider provider)
    {
        _sender = sender;

        EmailValidator = new EmailValidator();
        Password = string.Empty;
        HandleSubmitCommand = new CustomAsyncCommand(this, provider, HandleSubmit);
    }

    private async Task HandleSubmit()
    {
        try
        {
            if (EmailValidator.HasErrors)
            {
                EmailValidator.MapEmailErrors();
                return;
            }

            LoginCommand command = new(Username: EmailValidator.Email, Password: Password);
            
            IsBusy = true;
            ErrorOr<string> result = await _sender
                .Send(request: command)
                .ConfigureAwait(true);
            IsBusy = false;

            if (result.IsError)
            {
                Dialogs.Alert(string.Join(';', result.ErrorsOrEmptyList.Select(e => e.Description)), "Error", "Ok");
            }
            else
            {
                Dialogs.Alert(result.Value, "Success", "Ok");
            }
        }
        catch (Application.Exceptions.ValidationException ex)
        {
            foreach (Error error in ex.Errors)
            {
                switch (error.Code)
                {
                    case nameof(EmailValidator.Email):
                        EmailValidator.Error = string.Join(Environment.NewLine, error.Description);
                        break;
                    case nameof(Password):
                        Dialogs.Alert(string.Join(Environment.NewLine, error.Description), "Error", "Ok");
                        break;
                }
            }
        }

        throw new NotImplementedException();
    }

    public override void Initialize()
    {
        base.Initialize();
    }
}
