namespace Jha.Services.TweetsCollectorService.Services.Hanfire.Twitter;

using System;
using Hangfire.JobsLogger;
using Hangfire.Server;
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
    private readonly IStorage<Tweet> storage;
    private readonly ILogger<TwitterBackgroundJob> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterBackgroundJob"/> class.
    /// </summary>
    /// <param name="twitter">The injected Twitter service.</param>
    /// <param name="storage">The injected storage.</param>
    /// <param name="logger">The injected logger.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TwitterBackgroundJob(ITwitterService twitter, IStorage<Tweet> storage, ILogger<TwitterBackgroundJob> logger)
    {
        this.twitter = twitter ?? throw new ArgumentNullException(nameof(twitter));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
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
            this.logger.LogInformation("Background job '{Job}' started.", jobName);
            context.LogInformation($"Background job '{jobName}' started.");

            int pulledTweetsCount = 0;
            await foreach (var tweet in this.twitter.GetTweetsStream(context.CancellationToken.ShutdownToken))
            {
                if (tweet?.Data != null)
                {
                    this.storage.Add(new Tweet(tweet.Data));

                    pulledTweetsCount++;
                    if (pulledTweetsCount % 100 == 0)
                    {
                        this.logger.LogDebug("{Job} in progress... Pulled {N} tweets.", pulledTweetsCount);
                        context.LogDebug($"{jobName} in progress... Pulled {pulledTweetsCount} tweets.");
                    }
                }
                else
                {
                    this.logger.LogWarning("Pulled tweet data is invadid.");
                    context.LogWarning("Pulled tweet data is invadid.");
                }
            }

            this.logger.LogInformation("Background job '{Job}' completed. Pulled {N} tweets.", jobName, pulledTweetsCount);
            context.LogInformation($"Background job '{jobName}' started. Pulled {pulledTweetsCount} tweets.");
        }
        catch (TaskCanceledException)
        {
            this.logger.LogWarning("Background job '{Job}' canceled.", jobName);
            context.LogWarning($"Background job '{jobName}' canceled.");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Background job '{Job}' failed.", jobName);
            context.LogError($"Background job '{jobName}' failed: {ex.Message}.");
        }
    }

    #endregion
}

