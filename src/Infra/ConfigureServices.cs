using Infra.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nCroner.Common;
using nCroner.Core.Tools;
using Serilog;

namespace Infra;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        ConfigSeriLog(configuration);
        services.AddScoped<ICLogger, CLogger>();
        services.AddTransient<IDb, CronDb>();
    }

    private static void ConfigSeriLog(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
    }
    
}