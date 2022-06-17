using Application.Models;
using Application.Models.CronJobs;
using Application.Plugins;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        LoadPlugins(services);
        
        services.AddScoped<IValidator<CronJobModel>, CronJobModelValidator>();
    }

    private static void LoadPlugins(IServiceCollection services)
    {
        var pluginsProvider = new PluginsLoader(services);
        pluginsProvider.LoadPlugins();
        services.AddSingleton<IPluginsLoader>(pluginsProvider);
    }
}