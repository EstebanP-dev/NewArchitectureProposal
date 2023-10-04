using Domain.Errors;
using Domain.Features.Auth.Errors;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.Features.Auth;

public sealed record class LoginCommand(string Username, string Password)
    : ICommand<string>;

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

            HttpResponseMessage response = await client.PostAsJsonAsync(
                 requestUri: _options.General.Token,
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
    }
}
