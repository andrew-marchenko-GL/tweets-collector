namespace Jha.Services.TweetsCollectorService.Services.Hanfire.Twitter;

using Hangfire.Server;

/// <summary>
/// The Twitter background job interface
/// </summary>
public interface ITwitterBackgroundJob
{
    /// <summary>
    /// Starts a backgrond job that pulls tweets from Tweeter stream API
    /// and adds them to the storage.
    /// </summary>
    /// <param name="context">The background job perform context.</param>
    /// <returns>A task that represents asynchronious operation.</returns>
    Task PullTweetsIntoStorage(PerformContext? context);
}