using Application.Models;
using Application.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        LoadPlugins(services);
        
    }

    private static void LoadPlugins(IServiceCollection services)
    {
        var pluginsProvider = new PluginsLoader(services);
        pluginsProvider.LoadPlugins();
        services.AddSingleton<IPluginsLoader>(pluginsProvider);
    }
}