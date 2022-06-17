using nCroner.Core.Models;

namespace Application.Plugins;

public interface IPluginsLoader
{
    void LoadPlugins();
    IReadOnlyCollection<TriggerTypeDataModel> Triggers { get; }
    IReadOnlyCollection<TypeDataModel> Actions { get; }
    TriggerTypeDataModel? GetTrigger(Guid id);
    TypeDataModel? GetAction(Guid id);
}