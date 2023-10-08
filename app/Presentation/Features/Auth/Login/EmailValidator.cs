using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Presentation.Features.Auth.Login;


public sealed partial class EmailValidator
    : ObservableValidator
{
    public const int MinLength = 5;
    public const int MaxLength = 30;

    [GeneratedRegex(
    pattern: @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
    options: RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    [Required]
    [MinLength(MinLength)]
    [MaxLength(MaxLength)]
    [CustomValidation(typeof(EmailValidator), nameof(ValidateEmail))]
    [ObservableProperty]
    public string email;

    [ObservableProperty]
    public bool showError;

    [ObservableProperty]
    public string error;

    public EmailValidator()
    {
        ValidateProperties();
        ShowError = false;
        Error = string.Empty;
    }

    private void ValidateProperties()
    {
        ValidateAllProperties();
        MapEmailErrors();
    }

    public static ValidationResult ValidateEmail(string email, ValidationContext context)
    {
        if (!EmailRegex().IsMatch(email))
        {
            string text = LocalizationResourceHelper.Current["LoginResource.Email_Field_InvalidMatch"];
            return new(text);
        }

        return ValidationResult.Success;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(Email))
        {
            ValidateAllProperties();
            MapEmailErrors();
        }
    }

    public void MapEmailErrors()
    {
        if (HasErrors)
        {
            Error = string.Join(Environment.NewLine, GetErrors().Select(x => x.ErrorMessage));
            ShowError = true;
        }
        else
        {
            ShowError = false;
        }
    }
}
