using Application.Abstractions.Data;
using Application.Models.Settings;
using Microsoft.Extensions.Options;
using Presentation.Abstractions.Dependencies;

namespace MauiApp1.DelegatingHandlers;

internal class GeneralAuthenticationHandler : DelegatingHandler
{
    private readonly IPreferenceService _preferenceService;
    private readonly ApiSettings _apiSettings;

    public GeneralAuthenticationHandler(IPreferenceService preferenceService, IOptions<ApiSettings> options)
    {
        _preferenceService = preferenceService;
        _apiSettings = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string token = _preferenceService.Get("Token", string.Empty);

        Console.WriteLine(_apiSettings.BaseUrl);

        request.Headers.Add("Authentication", $"Bearer {token}");
        request.Headers.Add("Accept", "application/json");

        return base.SendAsync(request, cancellationToken);
    }
}
