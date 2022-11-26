namespace Jha.Services.TweetsCollectorService.Controllers.v1;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
using Jha.Services.TweetsCollectorService.Services.Twitter;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The Twitter API controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TwitterController : ControllerBase
{
    #region Private members

    private readonly IStorage<Tweet> storage;
    private readonly ITwitterService twitter;
    private readonly ILogger<TwitterController> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterController"/> class.
    /// </summary>
    /// <param name="storage">The injected tweets storage.</param>
    /// <param name="twitter">The injected Twitter service.</param>
    /// <param name="logger">The injected logger.</param>
    public TwitterController(IStorage<Tweet> storage, ITwitterService twitter, ILogger<TwitterController> logger)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.twitter = twitter ?? throw new ArgumentNullException(nameof(twitter));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    /// <summary>
    /// Count
    /// </summary>
    /// <remarks>
    /// Gets a tweets count in the storage.
    /// </remarks>
    /// <returns>Tweets count.</returns>
    [HttpGet("[action]")]
    public ActionResult<int> Count()
    {
        this.logger.LogInformation("{Action} request received.", nameof(this.Count));

        int result = this.storage.Count();

        this.logger.LogInformation("{Action} request completed. Fount {Count} tweets in the storage.", nameof(this.Count), result);
        return this.Ok(result);
    }

    /// <summary>
    /// Clear
    /// </summary>
    /// <remarks>
    /// Erases the Twitter storage.
    /// </remarks>
    /// <returns>Tweets count.</returns>
    [HttpDelete("[action]")]
    public IActionResult Clear()
    {
        this.logger.LogInformation("{Action} request received.", nameof(this.Clear));

        this.storage.Clear();

        this.logger.LogInformation("{Action} request completed.", nameof(this.Clear));
        return this.Ok();
    }

    /// <summary>
    /// PullTweets
    /// </summary>
    /// <remarks>
    /// Starts a background job that pulls tweets from Twitter basic stream API.
    /// </remarks>
    /// <returns>Test</returns>
    [HttpPost("[action]")]
    public async Task<ActionResult<IList<TweetBase>>> PullTweets()
    {
        using var cts = new CancellationTokenSource();
        int i = 0;
        var tweets = new List<TweetBase>();

        await foreach (var tweet in this.twitter.GetTweetsStream(cts.Token))
        {
            if (tweet?.Data != null)
            {
                tweets.Add(tweet.Data);
            }
            i++;

            if (i > 5)
            {
                cts.Cancel();
            }
        }

        return this.Ok(tweets);
    }
}

