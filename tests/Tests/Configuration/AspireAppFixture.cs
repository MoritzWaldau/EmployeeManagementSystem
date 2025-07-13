using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Tests.Configuration;

[CollectionDefinition("AspireApp")]
public class AspireAppCollection : ICollectionFixture<AspireAppFixture>
{
}

public sealed class AspireAppFixture :IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    private IDistributedApplicationTestingBuilder? _appHost;
    private DistributedApplication _app;

    public HttpClient ApiClient { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(cancellationToken);
        _appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(_appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
        });
        _appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    
        _app = await _appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await _app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        
        ApiClient = _app.CreateHttpClient("api");
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("api", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        var response = ApiClient.GetAsync($"api/data/2", cancellationToken);
        await response.WaitAsync(DefaultTimeout, cancellationToken);
    }

    public async Task DisposeAsync()
    {
        await _app.DisposeAsync();
    }
}