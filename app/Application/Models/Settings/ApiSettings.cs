namespace Application.Models.Settings;

public sealed class ApiSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public string WebApi { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public RegistrationEndpoints Registration { get; init; } = new(string.Empty);
    public GeneralEndpoints General { get; init; } = new(string.Empty);
}
public sealed record class RegistrationEndpoints(string AccountActivation);
public sealed record class GeneralEndpoints(string Token);
