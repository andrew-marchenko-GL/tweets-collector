namespace Jha.Services.TweetsCollectorService.Controllers.v1;

using System;
using Jha.Services.TweetsCollectorService.Models.Responses;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The tweet controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TweetController : ControllerBase
{
    #region Private members

    private readonly IRepository<Tweet> tweetsRepository;
    private readonly ILogger<TwitterController> logger;

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetController"/> class.
    /// </summary>
    /// <param name="tweetsRepository">The injected tweets repository.</param>
    /// <param name="logger">The injected logger.</param>
    public TweetController(IRepository<Tweet> tweetsRepository, ILogger<TwitterController> logger)
    {
        this.tweetsRepository = tweetsRepository ?? throw new ArgumentNullException(nameof(tweetsRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    /// <summary>
    /// GetTweet
    /// </summary>
    /// <remarks>
    /// Gets a tweet by the identifier from the storage.
    /// </remarks>
    /// <param name="id">Tweet identifier.</param>
    /// <returns>A tweet.</returns>
    [HttpGet("{id}")]
    public ActionResult<Tweet> GetTweet(string id)
    {
        this.logger.LogInformation(message: "{Action} request received. Tweet ID={Id}", nameof(this.GetTweet), id);

        if (string.IsNullOrWhiteSpace(id))
        {
            this.logger.LogWarning(message: "{Action} request completed. Request is invalid: 'id' is null or whitespace.", nameof(this.GetTweet));
            return this.BadRequest(new UnsuccessResonse("'id' is required."));
        }

        var result = this.tweetsRepository.GetFirstOrDefault(tweet => tweet.Id == id.ToLowerInvariant());
        if (result == null)
        {
            this.logger.LogWarning(message: "{Action} request completed. Tweet ID={Id} is not found in the storage.", nameof(this.GetTweet), id);
            return this.NotFound(new UnsuccessResonse($"Tweet with ID={id} is not found."));
        }

        this.logger.LogInformation(message: "{Action} request completed. Tweet found: {Tweet}", nameof(this.GetTweet), result.ToString());
        return this.Ok(result);
    }

    /// <summary>
    /// CreateTweet
    /// </summary>
    /// <remarks>
    /// Creates a new tweet.
    /// </remarks>
    /// <param name="tweet">Tweet text model.</param>
    /// <returns>A created tweet.</returns>
    [HttpPost]
    public ActionResult<Tweet> CreateTweet(TweetText? tweet)
    {
        this.logger.LogInformation(message: "{Action} request received. Tweet: {Tweet}", nameof(this.CreateTweet), tweet);

        if (tweet == null || !ModelState.IsValid)
        {
            this.logger.LogWarning(message: "{Action} request completed. Request is invalid: 'tweet' invalid.", nameof(this.CreateTweet));
            return this.BadRequest(new UnsuccessResonse("tweet invalid."));
        }

        var result = this.tweetsRepository.Add(new Tweet(tweet));

        this.logger.LogInformation(message: "{Action} request completed. Tweet created: {Tweet}", nameof(this.CreateTweet), result.ToString());
        return this.Ok(result);
    }

    /// <summary>
    /// DeleteTweet
    /// </summary>
    /// <remarks>
    /// Deletes an existing tweet.
    /// </remarks>
    /// <param name="id">Tweet identifier.</param>
    /// <returns>A deleted tweet.</returns>
    [HttpDelete("{id}")]
    public ActionResult<Tweet> DeleteTweet(string id)
    {
        this.logger.LogInformation(message: "{Action} request received. Tweet ID={ID}", nameof(this.DeleteTweet), id);

        if (string.IsNullOrWhiteSpace(id))
        {
            this.logger.LogWarning(message: "{Action} request completed. Request is invalid: 'id' is null or whitespace.", nameof(this.DeleteTweet));
            return this.BadRequest(new UnsuccessResonse("'id' is required."));
        }

        var result = this.tweetsRepository.RemoveFirstOrDefault(tweet => tweet.Id == id.ToLowerInvariant());
        if (result == null)
        {
            this.logger.LogWarning(message: "{Action} request completed. Tweet ID={Id} is not found in the storage.", nameof(this.DeleteTweet), id);
            return this.NotFound(new UnsuccessResonse($"Tweet with ID={id} is not found."));
        }

        this.logger.LogInformation(message: "{Action} request completed. Tweet created: {Tweet}", nameof(this.DeleteTweet), result.ToString());
        return this.Ok(result);
    }
}
