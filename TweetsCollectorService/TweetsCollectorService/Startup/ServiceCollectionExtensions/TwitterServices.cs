﻿namespace Jha.Services.TweetsCollectorService.Startup.ServiceCollectionExtensions;

using System;
using Jha.Services.TweetsCollectorService.Models.Configuration;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
using Jha.Services.TweetsCollectorService.Services.Twitter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;

/// <summary>
/// The Twitter services extensions.
/// </summary>
public static class TwitterServices
{
    /// <summary>
    /// Adds Twitter related services to the container.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns>Services collection.</returns>
    public static IServiceCollection AddTwitterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ITwitterService, TwitterService>();
        services.Configure<TwitterConfiguration>(options => configuration.GetSection(TwitterConfiguration.SectionPath).Bind(options));
        services.AddSingleton<IStorage<Tweet>, TwitterStorage>();

        return services;
    }
}

