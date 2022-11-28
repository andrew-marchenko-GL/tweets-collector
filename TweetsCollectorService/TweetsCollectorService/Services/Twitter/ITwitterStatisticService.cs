namespace Jha.Services.TweetsCollectorService.Services.Twitter;

using Jha.Services.TweetsCollectorService.Models.Twitter;

/// <summary>
/// The Twitter statistic service interface.
/// </summary>
public interface ITwitterStatisticService
{
    /// <summary>
    /// Generates the statistic report.
    /// </summary>
    /// <returns>The report.</returns>
    TweetStatReport GenerateReport();
}