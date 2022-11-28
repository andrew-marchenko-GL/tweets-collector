namespace Jha.Services.TweetsCollectorService.Services.Hangfire.Twitter;

using System;
using global::Hangfire.JobsLogger;
using global::Hangfire.Server;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
using Jha.Services.TweetsCollectorService.Services.Twitter;

/// <summary>
/// The Twitter related tasks background jobs service.
/// </summary>
public class TwitterBackgroundJob : ITwitterBackgroundJob
{
    #region Private members

    private readonly ITwitterService twitter;
    private readonly IRepository<Tweet> tweetsRepository;
    private readonly ILogger<TwitterBackgroundJob> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterBackgroundJob"/> class.
    /// </summary>
    /// <param name="twitter">The injected Twitter service.</param>
    /// <param name="tweetsRepository">The injected tweets repository.</param>
    /// <param name="logger">The injected logger.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TwitterBackgroundJob(ITwitterService twitter, IRepository<Tweet> tweetsRepository, ILogger<TwitterBackgroundJob> logger)
    {
        this.twitter = twitter ?? throw new ArgumentNullException(nameof(twitter));
        this.tweetsRepository = tweetsRepository ?? throw new ArgumentNullException(nameof(tweetsRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region ITwitterBackgroundJob

    /// <inheritdoc/>
    public async Task PullTweetsIntoStorage(PerformContext? context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        string jobName = nameof(this.PullTweetsIntoStorage);

        try
        {
            this.logger.LogInformation(message: "Background job '{JobName}' started.", jobName);
            context.LogInformation($"Background job '{jobName}' started.");

            int pulledTweetsCount = 0;
            while (!context.CancellationToken.ShutdownToken.IsCancellationRequested)
            {
                await foreach (var tweetResponse in this.twitter.GetTweetsStream(context.CancellationToken.ShutdownToken))
                {
                    if (tweetResponse.Data != null)
                    {
                        var tweet = new Tweet(tweetResponse.Data);
                        tweet.Meta.TransactionId = context.BackgroundJob.Id;
                        this.tweetsRepository.Add(tweet);

                        pulledTweetsCount++;
                        if (pulledTweetsCount % 100 == 0) // Log each 100 tweets count
                        {
                            this.logger.LogDebug(message: "{Job} in progress... Pulled {N} tweets.", jobName, pulledTweetsCount);
                            context.LogDebug($"{jobName} in progress... Pulled {pulledTweetsCount} tweets.");
                        }
                    }
                    else
                    {
                        this.logger.LogWarning(message: "Pulled tweet data is invadid.");
                        context.LogWarning("Pulled tweet data is invadid.");
                    }
                }

                // This delay is needed to cover Twitter API dev single connection limitation.
                await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken.ShutdownToken);
            }

            this.logger.LogInformation(message: "Background job '{JobName}' completed. Pulled {PulledTweetsCount} tweets.", jobName, pulledTweetsCount);
            context.LogInformation($"Background job '{jobName}' completed. Pulled {pulledTweetsCount} tweets.");
        }
        catch (TaskCanceledException)
        {
            this.logger.LogWarning(message: "Background job '{JobName}' canceled.", jobName);
            context.LogWarning($"Background job '{jobName}' canceled.");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, message: "Background job '{JobName}' failed.", jobName);
            context.LogError($"Background job '{jobName}' failed: {ex.Message}.");
            throw;
        }
    }

    #endregion
}
