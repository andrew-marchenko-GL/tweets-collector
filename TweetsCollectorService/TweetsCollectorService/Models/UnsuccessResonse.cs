namespace Jha.Services.TweetsCollectorService.Models;

using System;

/// <summary>
/// The unsuccess reponse model.
/// </summary>
public class UnsuccessResonse
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsuccessResonse"/> class.
    /// </summary>
    public UnsuccessResonse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsuccessResonse"/> class.
    /// </summary>
    /// <param name="message">The response message.</param>
    /// <exception cref="ArgumentNullException">When argument is not supplied.</exception>
    public UnsuccessResonse(string message)
    {
        this.Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    #endregion

    /// <summary>
    /// Gets or sets whether response is success.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the response message.
    /// </summary>
    public string? Message { get; set; }
}

