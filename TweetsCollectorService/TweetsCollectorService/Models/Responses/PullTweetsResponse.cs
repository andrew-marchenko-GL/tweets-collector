namespace Jha.Services.TweetsCollectorService.Models.Responses;

using System;

/// <summary>
/// The pull tweets response model.
/// </summary>
public class PullTweetsResponse
{
    /// <summary>
    /// Indicates whether response is success.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the background job identifier.
    /// </summary>
    public string? JobId { get; set; }

    /// <summary>
    /// Gets or sets the background job dashboard URL.
    /// </summary>
    public Uri? JobUri { get; set; }
}

