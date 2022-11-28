namespace Jha.Services.TweetsCollectorService.Services.Storage;

using System;
using System.Collections.Concurrent;
using System.Globalization;
using Jha.Services.TweetsCollectorService.Models.Twitter;

/// <summary>
/// The Twitter tweets in memory storage.
/// </summary>
public class TwitterStorage : IStorage<Tweet>
{
    #region Private members

    private readonly ConcurrentDictionary<string, Tweet> storage;
    private readonly ILogger<TwitterStorage> logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterStorage"/> class.
    /// </summary>
    /// <param name="logger">The injected logger.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TwitterStorage(ILogger<TwitterStorage> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.storage = new ConcurrentDictionary<string, Tweet>();
    }

    #endregion

    #region IStorage

    /// <inheritdoc/>
    public Tweet Add(Tweet entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            entity.Id = (this.storage.Count + 1).ToString(CultureInfo.InvariantCulture);
        }

        if (!this.storage.TryAdd(entity.Id, entity))
        {
            throw new ArgumentException($"The tweet with ID={entity.Id} already exists.");
        }

        this.logger.LogInformation("Tweet added to the storage. Tweet ID={Id}", entity.Id);

        return entity;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        this.storage.Clear();
        this.logger.LogInformation("Tweet storage cleared.");
    }

    /// <inheritdoc/>
    public int Count()
    {
        return this.storage.Count;
    }

    /// <inheritdoc/>
    public IEnumerable<Tweet> GetAllWhere(Func<Tweet, bool>? predicate = null)
    {
        return predicate == null ? this.storage.Values : this.storage.Values.Where(predicate) ?? new List<Tweet>();
    }

    /// <inheritdoc/>
    public Tweet? GetFirstOrDefault(Func<Tweet, bool> predicate)
    {
        return this.storage.Values.FirstOrDefault(predicate);
    }

    /// <inheritdoc/>
    public Tweet? RemoveFirstOrDefault(Func<Tweet, bool> predicate)
    {
        var tweet = this.storage.Values.FirstOrDefault(predicate);
        if (tweet?.Id != null)
        {
            if (!this.storage.Remove(tweet.Id, out _))
            {
                this.logger.LogWarning("Unable to remove tweet {Tweet}.", tweet);
            }

            return tweet;
        }

        return null;
    }

    #endregion
}

