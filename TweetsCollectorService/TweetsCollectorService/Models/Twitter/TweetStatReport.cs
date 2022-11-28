namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;

/// <summary>
/// The Twitter statisic report model.
/// </summary>
public class TweetStatReport
{
    /// <summary>
    /// Gets or sets the oldest tweet time (UTC)
    /// </summary>
    public DateTime OldestTweetAddedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the youngest tweet time (UTC)
    /// </summary>
    public DateTime YoungestTweetAddedAt { get; set; }

    /// <summary>
    /// Gets or sets the total tweets number.
    /// </summary>
    public int TotalTweets { get; set; }

    /// <summary>
    /// Gets or sets the tweets added per minute.
    /// </summary>
    public double TweetsPerMinute { get; set; }

    /// <summary>
    /// Gets or sets the minum tweet length.
    /// </summary>
    public int MinTweetLength { get; set; }

    /// <summary>
    /// Gets or sets the maximum tweet length.
    /// </summary>
    public int MaxTweetLength { get; set; }

    /// <summary>
    /// Gets or sets the average tweet length.
    /// </summary>
    public int AvgTweetLength { get; set; }
}

