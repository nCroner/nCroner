using Microsoft.Extensions.DependencyInjection;
using nCroner.Common.Models;

namespace Application.Plugins;

public interface IPluginsLoader
{
    void LoadPlugins();
    IReadOnlyCollection<TriggerTypeDataModel> Triggers { get; }
    IReadOnlyCollection<TypeDataModel> Actions { get; }
}