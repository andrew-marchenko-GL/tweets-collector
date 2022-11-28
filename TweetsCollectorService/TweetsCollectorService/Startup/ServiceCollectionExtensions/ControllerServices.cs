namespace Jha.Services.TweetsCollectorService.Startup.ServiceCollectionExtensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

/// <summary>
/// Controllers services extensions.
/// </summary>
public static class ControllerServices
{
    /// <summary>
    /// Adds ASP.Net controller related services to the container.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <returns>Services collection.</returns>
    public static IServiceCollection AddControllerServices(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
            });
        services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}

