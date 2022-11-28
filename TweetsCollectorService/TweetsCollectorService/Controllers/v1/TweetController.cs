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

    private readonly IStorage<Tweet> storage;
    private readonly ILogger<TwitterController> logger;

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetController"/> class.
    /// </summary>
    /// <param name="storage">The injected tweets storage.</param>
    /// <param name="logger">The injected logger.</param>
    public TweetController(IStorage<Tweet> storage, ILogger<TwitterController> logger)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    /// <summary>
    /// GetTweetById
    /// </summary>
    /// <remarks>
    /// Gets a tweet by the identifier from the storage.
    /// </remarks>
    /// <param name="id">Tweet identifier.</param>
    /// <returns>A tweet.</returns>
    [HttpGet("{id}")]
    public ActionResult<Tweet> GetTweetById(string id)
    {
        this.logger.LogInformation("{Action} request received. Tweet ID={Id}", nameof(this.GetTweetById), id);

        if (string.IsNullOrWhiteSpace(id))
        {
            this.logger.LogWarning("{Action} request completed. Request is invalid: 'id' is null or whitespace.", nameof(this.GetTweetById));
            return this.BadRequest(new UnsuccessResonse("'id' is required."));
        }

        var result = this.storage.GetFirstOrDefault(tweet => tweet.Id == id.ToLowerInvariant());
        if (result == null)
        {
            this.logger.LogWarning("{Action} request completed. Tweet ID={Id} is not found in the storage.", nameof(this.GetTweetById), id);
            return this.NotFound(new UnsuccessResonse($"Tweet with ID={id} is not found."));
        }

        this.logger.LogInformation("{Action} request completed. Tweet found: {Tweet}", nameof(this.GetTweetById), result.ToString());
        return this.Ok(result);
    }

    /// <summary>
    /// CreateTweet
    /// </summary>
    /// <remarks>
    /// Creates a new tweet.
    /// </remarks>
    /// <param name="tweet">Tweet text model.</param>
    /// <returns>A tweet.</returns>
    [HttpPost]
    public ActionResult<Tweet> CreateTweet(TweetText tweet)
    {
        this.logger.LogInformation("{Action} request received. Tweet: {Tweet}", nameof(this.CreateTweet), tweet);

        if (tweet == null || !ModelState.IsValid)
        {
            this.logger.LogWarning("{Action} request completed. Request is invalid: 'tweet' invalid.", nameof(this.CreateTweet));
            return this.BadRequest(new UnsuccessResonse("tweet invalid."));
        }

        var result = this.storage.Add(new Tweet(tweet));

        this.logger.LogInformation("{Action} request completed. Tweet created: {Tweet}", nameof(this.CreateTweet), result.ToString());
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
        this.logger.LogInformation("{Action} request received. Tweet ID={ID}", nameof(this.DeleteTweet), id);

        if (string.IsNullOrWhiteSpace(id))
        {
            this.logger.LogWarning("{Action} request completed. Request is invalid: 'id' is null or whitespace.", nameof(this.DeleteTweet));
            return this.BadRequest(new UnsuccessResonse("'id' is required."));
        }

        var result = this.storage.RemoveFirstOrDefault(tweet => tweet.Id == id.ToLowerInvariant());
        if (result == null)
        {
            this.logger.LogWarning("{Action} request completed. Tweet ID={Id} is not found in the storage.", nameof(this.DeleteTweet), id);
            return this.NotFound(new UnsuccessResonse($"Tweet with ID={id} is not found."));
        }

        this.logger.LogInformation("{Action} request completed. Tweet created: {Tweet}", nameof(this.DeleteTweet), result.ToString());
        return this.Ok(result);
    }
}

