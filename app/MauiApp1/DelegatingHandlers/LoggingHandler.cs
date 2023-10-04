using Presentation.Abstractions.Dependencies;

namespace MauiApp1.DelegatingHandlers;

internal sealed class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Before HTTP Request.");

            HttpResponseMessage result = await base.SendAsync(request, cancellationToken);

            result.EnsureSuccessStatusCode();

            Console.WriteLine("After HTTP Request.");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // TODO: Change for logging.

            throw;
        }
    }
}
