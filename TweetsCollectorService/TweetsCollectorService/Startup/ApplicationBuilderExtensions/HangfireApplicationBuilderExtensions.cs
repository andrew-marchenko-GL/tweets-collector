namespace Jha.Services.TweetsCollectorService.Startup.ApplicationBuilderExtensions;

using Hangfire;

/// <summary>
/// Extensions to configure Hangfire.
/// </summary>
public static class HangfireApplicationBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAndConfigureHanfire(this IApplicationBuilder builder)
    {
        builder.UseHangfireDashboard();

        return builder;
    }
}

