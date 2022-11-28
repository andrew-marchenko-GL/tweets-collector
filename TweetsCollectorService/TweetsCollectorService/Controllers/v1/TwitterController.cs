namespace Jha.Services.TweetsCollectorService.Controllers.v1;

using System;
using Hangfire;
using Jha.Services.TweetsCollectorService.Models.Responses;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Hangfire.Twitter;
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

    private readonly IRepository<Tweet> tweetsRepository;
    private readonly IBackgroundJobClient backgroundJobClient;
    private readonly ITwitterBackgroundJob twitterBackgroundJob;
    private readonly ITwitterStatisticService twitterStatisticService;
    private readonly ILogger<TwitterController> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterController"/> class.
    /// </summary>
    /// <param name="tweetsRepository">The injected tweets repository.</param>
    /// <param name="backgroundJobClient">The injected Hangfire background job client.</param>
    /// <param name="twitterBackgroundJob">The injected Twitter background job.</param>
    /// <param name="twitterStatisticService">The injected Twitter statistic service.</param>
    /// <param name="logger">The injected logger.</param>
    public TwitterController(
        IRepository<Tweet> tweetsRepository,
        IBackgroundJobClient backgroundJobClient,
        ITwitterBackgroundJob twitterBackgroundJob,
        ITwitterStatisticService twitterStatisticService,
        ILogger<TwitterController> logger)
    {
        this.tweetsRepository = tweetsRepository ?? throw new ArgumentNullException(nameof(tweetsRepository));
        this.backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
        this.twitterBackgroundJob = twitterBackgroundJob ?? throw new ArgumentNullException(nameof(twitterBackgroundJob));
        this.twitterStatisticService = twitterStatisticService ?? throw new ArgumentNullException(nameof(twitterStatisticService));
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
        this.logger.LogInformation(message: "{Action} request received.", nameof(this.Count));

        int result = this.tweetsRepository.Count();

        this.logger.LogInformation(message: "{Action} request completed. Found {Count} tweets in the storage.", nameof(this.Count), result);
        return this.Ok(result);
    }

    /// <summary>
    /// Clear
    /// </summary>
    /// <remarks>
    /// Erases the Twitter storage.
    /// </remarks>
    [HttpDelete("[action]")]
    public IActionResult Clear()
    {
        this.logger.LogInformation(message: "{Action} request received.", nameof(this.Clear));

        this.tweetsRepository.Clear();

        this.logger.LogInformation(message: "{Action} request completed.", nameof(this.Clear));
        return this.Ok();
    }

    /// <summary>
    /// StartPolling
    /// </summary>
    /// <remarks>
    /// Starts a background job that pulls tweets from Twitter basic stream API.
    /// </remarks>
    /// <returns>The response with information about background job.</returns>
    [HttpPost("[action]")]
    public ActionResult<PullTweetsResponse> StartPolling()
    {
        this.logger.LogInformation(message: "{Action} request received.", nameof(this.StartPolling));

        string jobId = this.backgroundJobClient.Enqueue(() => this.twitterBackgroundJob.PullTweetsIntoStorage(null));
        var jobUri = new Uri($"{this.Request.Scheme}://{this.Request.Host.Value}/hangfire/jobs/details/{jobId}");
        var result = new PullTweetsResponse { IsSuccess = true, JobId = jobId, JobUri = jobUri };

        this.logger.LogInformation(message: "{Action} request completed. Enqueued job '{JobId}'.", nameof(this.StartPolling), jobId);
        return this.Created(jobUri, result);
    }
    
    /// <summary>
    /// StopPolling
    /// </summary>
    /// <param name="jobId">The job identifier.</param>
    /// <returns></returns>
    [HttpPost("[action]/{jobId}")]
    public ActionResult<PullTweetsResponse> StopPolling(string jobId)
    {
        bool isSuccess = this.backgroundJobClient.Delete(jobId);
        var jobUri = new Uri($"{this.Request.Scheme}://{this.Request.Host.Value}/hangfire/jobs/details/{jobId}");
        var result = new PullTweetsResponse { IsSuccess = isSuccess, JobId = jobId, JobUri = jobUri };

        return isSuccess ? this.Ok(result) : this.BadRequest(result);
    }

    /// <summary>
    /// Statistic
    /// </summary>
    /// <remarks>
    /// Generates a statistics report of the Twitter storage.
    /// </remarks>
    /// <returns>The generated report.</returns>
    [HttpGet("[action]")]
    public ActionResult<TweetStatReport> Statistic()
    {
        return this.twitterStatisticService.GenerateReport();
    }
}
