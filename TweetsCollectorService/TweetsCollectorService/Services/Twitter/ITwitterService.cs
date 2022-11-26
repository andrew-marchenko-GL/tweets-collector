namespace Jha.Services.TweetsCollectorService.Services.Twitter;

using System.Runtime.CompilerServices;
using Jha.Services.TweetsCollectorService.Models.Twitter;

/// <summary>
/// The Twitter service interface.
/// </summary>
public interface ITwitterService
{
    /// <summary>
    /// Gets sample streamed tweets.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Streamed sample tweets.</returns>
    IAsyncEnumerable<TweetResponse<TweetBase>> GetTweetsStream(CancellationToken cancellationToken);
}