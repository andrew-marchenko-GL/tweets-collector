namespace Jha.Services.TweetsCollectorService.Controllers.v1;

using System;
using System.Linq;
using Hangfire;
using Jha.Services.TweetsCollectorService.Models.Responses;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Hanfire.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
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
    private readonly IBackgroundJobClient backgroundJobClient;
    private readonly ITwitterBackgroundJob twitterBackgroundJob;
    private readonly ILogger<TwitterController> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterController"/> class.
    /// </summary>
    /// <param name="storage">The injected tweets storage.</param>
    /// <param name="backgroundJobClient">The injected Hangfier background job client.</param>
    /// <param name="twitterBackgroundJob">The injected Twitter background job.</param>
    /// <param name="logger">The injected logger.</param>
    public TwitterController(
        IStorage<Tweet> storage,
        IBackgroundJobClient backgroundJobClient,
        ITwitterBackgroundJob twitterBackgroundJob,
        ILogger<TwitterController> logger)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
        this.twitterBackgroundJob = twitterBackgroundJob ?? throw new ArgumentNullException(nameof(twitterBackgroundJob));
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
    public ActionResult<PullTweetsResponse> PullTweets()
    {
        this.logger.LogInformation("{Action} request received.", nameof(this.PullTweets));

        string jobId = this.backgroundJobClient.Enqueue(() => this.twitterBackgroundJob.PullTweetsIntoStorage(null));
        var jobUri = new Uri($"{this.Request.Scheme}://{this.Request.Host.Value}/hangfire/jobs/details/{jobId}");
        var result = new PullTweetsResponse { IsSuccess = true, JobId = jobId, JobUri = jobUri };

        this.logger.LogInformation("{Action} request completed. Enqued job '{JobId}'.", nameof(this.PullTweets), jobId);

        return this.Created(jobUri, result);
    }
}

