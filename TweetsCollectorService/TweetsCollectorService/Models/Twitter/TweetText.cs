namespace Jha.Services.TweetsCollectorService.Models.Twitter;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The tweet text model.
/// </summary>
public class TweetText
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetText"/> class.
    /// </summary>
    public TweetText()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TweetText"/> class.
    /// </summary>
    /// <param name="text">The tweet text</param>
    /// <exception cref="ArgumentNullException">When argument is not supplied.</exception>
    public TweetText(string text)
    {
        this.Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    #endregion

    /// <summary>
    /// Gets or sets the tweet text.
    /// </summary>
    [Required]
    public string? Text { get; set; }
}

