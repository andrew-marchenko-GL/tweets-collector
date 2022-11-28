namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;

/// <summary>
/// The tweet metadata model.
/// </summary>
public class TweetMeta
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetMeta"/> class.
    /// </summary>
    /// <param name="tweet">The base tweet instance.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TweetMeta(TweetBase tweet)
    {
        if (tweet == null)
        {
            throw new ArgumentNullException(nameof(tweet));
        }

        this.TweetLength = tweet.Text?.Length ?? 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetMeta"/> class.
    /// </summary>
    /// <param name="tweet">The tweet text instance.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TweetMeta(TweetText tweet)
    {
        if (tweet == null)
        {
            throw new ArgumentNullException(nameof(tweet));
        }

        this.TweetLength = tweet.Text?.Length ?? 0;

    }

    #endregion

    /// <summary>
    /// Gets a time when tweet created.
    /// </summary>
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the tweet length
    /// </summary>
    public int TweetLength { get; }

    /// <summary>
    /// Gets or sets the transaction identifier.
    /// </summary>
    public string? TransactionId { get; set; }
}

