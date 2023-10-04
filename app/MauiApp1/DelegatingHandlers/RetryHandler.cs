using Polly;
using Polly.Retry;

namespace MauiApp1.DelegatingHandlers;

internal sealed class RetryHandler : DelegatingHandler
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .RetryAsync(3);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        PolicyResult<HttpResponseMessage> policyResult = await _retryPolicy.ExecuteAndCaptureAsync(
            () => base.SendAsync(request, cancellationToken));

        if (policyResult.Outcome == OutcomeType.Failure)
        {
            throw policyResult.FinalException as HttpRequestException;
        }

        return policyResult.Result;
    }
}
