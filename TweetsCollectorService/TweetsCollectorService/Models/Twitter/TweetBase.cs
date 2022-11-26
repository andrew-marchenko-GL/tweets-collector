namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;

/// <summary>
/// Represends the tweet basic entity.
/// </summary>
public class TweetBase
{
    /// <summary>
    /// Gets or sets the tweet identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the tweet text.
    /// </summary>
    public string? Text { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Id ?? string.Empty;
    }
}

