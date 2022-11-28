namespace Jha.Services.TweetsCollectorService.Services.Twitter;

using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Jha.Services.TweetsCollectorService.Models.Configuration;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Microsoft.Extensions.Options;

/// <summary>
/// The Twitter API client.
/// </summary>
public class TwitterService : ITwitterService
{
    #region Private members

    private readonly HttpClient client;
    private readonly JsonSerializerOptions serializerOptions;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterService"/> class.
    /// </summary>
    /// <param name="client">The injected HTTP client.</param>
    /// <param name="configuration">The injected </param>
    /// <exception cref="ArgumentNullException">Argument is not injected.</exception>
    public TwitterService(HttpClient client, IOptions<TwitterConfiguration> configuration)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));

        if (configuration?.Value == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        this.client.BaseAddress = configuration.Value.BaseUri;
        this.client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", configuration.Value.BearerToken);

        this.serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    #endregion

    #region ITwitterService

    /// <inheritdoc/>
    public async IAsyncEnumerable<TweetResponse<TweetBase>> GetTweetsStream([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var response = await this.client.GetAsync("2/tweets/sample/stream", HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        response.EnsureSuccessStatusCode();
        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var streamReader = new StreamReader(stream);

        string? lineResponse;
        do
        {
            lineResponse = await streamReader.ReadLineAsync();
            Console.WriteLine($"Line Response: {lineResponse}");
            if (!string.IsNullOrWhiteSpace(lineResponse))
            {
                var tweet = JsonSerializer.Deserialize<TweetResponse<TweetBase>>(lineResponse, this.serializerOptions);
                if (tweet != null)
                {
                    yield return tweet;
                }
            }
        } while (!string.IsNullOrWhiteSpace(lineResponse) && !cancellationToken.IsCancellationRequested);
    }

    #endregion
}

