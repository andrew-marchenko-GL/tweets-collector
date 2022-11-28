namespace Jha.Services.TweetsCollectorService.Startup.ServiceCollectionExtensions;

using Hangfire;
using Hangfire.JobsLogger;
using Hangfire.MemoryStorage;
using Jha.Services.TweetsCollectorService.Services.Hanfire.Twitter;

/// <summary>
/// The Hangfire services extensions.
/// </summary>
public static class HangfireServices
{
    /// <summary>
    /// Adds Hangfire related services to the container.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <returns>Services collection.</returns>
    public static IServiceCollection AddHangfireServices(this IServiceCollection services)
    {
        // Add Hangfire services
        services.AddHangfire(configuration =>
            {
                configuration.UseMemoryStorage();
                configuration.UseJobsLogger();
            });

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        // Add background job services
        services.AddScoped<ITwitterBackgroundJob, TwitterBackgroundJob>();

        return services;
    }
}

