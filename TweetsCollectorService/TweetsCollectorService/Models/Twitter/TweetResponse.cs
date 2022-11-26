namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;

/// <summary>
/// The tweet response model.
/// </summary>
/// <typeparam name="T">Tweet response data class.</typeparam>
public class TweetResponse<T>
{
    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public T? Data { get; set; }
}

