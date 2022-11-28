namespace Jha.Services.TweetsCollectorService.Models.Configuration;

using System;

/// <summary>
/// Model represents a Twitter services configuration.
/// </summary>
public class TwitterConfiguration
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionPath = "Twitter";

    /// <summary>
    /// Gets or sets the base Twitter API URL.
    /// </summary>
    public Uri? BaseUri { get; set; }

    /// <summary>
    /// Gets or sets the bearer auth token.
    /// </summary>
    public string? BearerToken { get; set; }
}

