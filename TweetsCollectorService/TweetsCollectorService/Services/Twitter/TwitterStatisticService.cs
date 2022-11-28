namespace Jha.Services.TweetsCollectorService.Services.Twitter;

using System;
using System.Linq;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;

/// <summary>
/// The Twitter statisctic service.
/// </summary>
public class TwitterStatisticService : ITwitterStatisticService
{
    #region Private members

    private readonly IRepository<Tweet> tweetsRepository;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterStatisticService"/> class.
    /// </summary>
    /// <param name="tweetsRepository">The injected twitter repository service.</param>
    public TwitterStatisticService(IRepository<Tweet> tweetsRepository)
    {
        this.tweetsRepository = tweetsRepository ?? throw new ArgumentNullException(nameof(tweetsRepository));
    }

    /// <inheritdoc/>
    public TweetStatReport GenerateReport()
    {
        int count = this.tweetsRepository.Count();
        if (count < 2)
        {
            return new TweetStatReport { TotalTweets = count };
        }

        var maxTime = this.tweetsRepository.Max(tweet => tweet.Meta.CreatedAtUtc);
        var minTime = this.tweetsRepository.Min(tweet => tweet.Meta.CreatedAtUtc);
        double throughput = count / (maxTime - minTime).TotalMinutes;
        int minTweetLength = this.tweetsRepository.Where(tweet => tweet.Meta.CreatedAtUtc <= maxTime).Min(tweet => tweet.Meta.TweetLength);
        int maxTweetLength = this.tweetsRepository.Where(tweet => tweet.Meta.CreatedAtUtc <= maxTime).Max(tweet => tweet.Meta.TweetLength);
        int avgTweetLength = (int)this.tweetsRepository.Where(tweet => tweet.Meta.CreatedAtUtc <= maxTime).Average(tweet => tweet.Meta.TweetLength);

        return new TweetStatReport
        {
            OldestTweetAddedAtUtc = minTime,
            YoungestTweetAddedAt = maxTime,
            TotalTweets = count,
            TweetsPerMinute = throughput,
            MinTweetLength = minTweetLength,
            MaxTweetLength = maxTweetLength,
            AvgTweetLength = avgTweetLength
        };
    }
}
