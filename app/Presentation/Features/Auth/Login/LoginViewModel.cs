using Acr.UserDialogs;
using Application.Features.Auth;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.Features.Auth.Login;

public sealed partial class LoginViewModel : ViewModelBase
{
    private readonly ISender _sender;

    [ObservableProperty]
    EmailValidator emailValidator;

    [ObservableProperty]
    public string password;

    public LoginViewModel(ISender sender)
    {
        _sender = sender;

        EmailValidator = new EmailValidator();
        Password = string.Empty;
    }

    [RelayCommand]
    public async Task HandleSubmit()
    {
        if (EmailValidator.HasErrors)
            return;

        LoginCommand command = new(Username: EmailValidator.Email, Password: Password);

        ErrorOr<string> result = await _sender
            .Send(request: command)
            .ConfigureAwait(true);

        if (result.IsError)
        {
            Dialogs.Alert(string.Join(';', result.ErrorsOrEmptyList.Select(e => e.Description)), "Error", "Ok");
        }
        else
        {
            Dialogs.Alert(result.Value, "Success", "Ok");
        }
    }
}
