using Domain.Errors;
using Domain.Features.Auth.Errors;
using Domain.Helpers;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.Features.Auth;

public sealed record class LoginCommand(string Username, string Password)
    : ICommand<string>;

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode("Validation.Email.Empty")
            .WithMessage(LocalizationResourceHelper.Current["LoginResorce.Email_Field_Empty"]);

        RuleFor(x => x.Username)
            .EmailAddress()
            .WithErrorCode("Validation.Email.Invalid")
            .WithMessage(LocalizationResourceHelper.Current["LoginResorce.Email_Field_InvalidMatch"]);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly ApiSettings _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginCommandHandler(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(nameof(ApiSettings.General));

            var obj = client.BaseAddress;

            HttpResponseMessage response = await client.PostAsJsonAsync(
                 requestUri: _options.WebApi + _options.Version + _options.General.Token,
                 value: request,
                 cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string stringMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                return DomainErrors.MapFromHttpResultMessage(response.StatusCode, stringMessage);
            }

            string stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            string? token = JsonConvert.DeserializeObject<string>(stringResponse);

            if (token is null)
            {
                return AuthErrors.Login.NullValue;
            }

            return token;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message); // TODO: Change for logging.
            return DomainErrors.UnExceptedError;
        }
        catch (HttpRequestException ex)
        {
            return DomainErrors.MapFromHttpResultMessage(ex.StatusCode ?? HttpStatusCode.InternalServerError, ex.Message);
        }
        catch (Exceptions.ValidationException ex)
        {
            return DomainErrors.MapFromListOfErrors(ex.Errors);
        }
    }
}
