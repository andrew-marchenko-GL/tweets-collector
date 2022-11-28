namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;

/// <summary>
/// The tweet model.
/// </summary>
public class Tweet : TweetBase
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Tweet"/> class.
    /// </summary>
    /// <param name="tweet">The basic tweet instance.</param>
    /// <exception cref="ArgumentNullException">When argument is not supplied.</exception>
    public Tweet(TweetBase tweet)
    {
        if (tweet == null)
        {
            throw new ArgumentNullException(nameof(tweet));
        }

        this.Id = tweet.Id;
        this.Text = tweet.Text;
        this.Meta = new TweetMeta(tweet);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tweet"/> class.
    /// </summary>
    /// <param name="tweet">The tweet text instance.</param>
    /// <exception cref="ArgumentNullException">When argument is not supplied.</exception>
    public Tweet(TweetText tweet)
    {
        if (tweet == null)
        {
            throw new ArgumentNullException(nameof(tweet));
        }

        this.Text = tweet.Text;
        this.Meta = new TweetMeta(tweet);
    }

    #endregion

    /// <summary>
    /// Gets the tweet metadata.
    /// </summary>
    public TweetMeta Meta { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return base.ToString();
    }
}

