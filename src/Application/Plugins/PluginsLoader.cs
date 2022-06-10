using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using nCroner.Common.Models;
using nCroner.Common.Plugins;

namespace Application.Plugins
{
    public class PluginsLoader : IPluginsLoader
    {
        private readonly List<TriggerTypeDataModel> _triggerPlugins = new();
        private readonly List<TypeDataModel> _actionPlugins = new();
        private readonly Dictionary<string, Assembly> _assemblies = new();

        private readonly IServiceCollection _services;

        public PluginsLoader(IServiceCollection services)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _services = services;
        }

        public void LoadPlugins()
        {
            

            _triggerPlugins.Clear();
            _actionPlugins.Clear();
            _assemblies.Clear();

            Log("Loading plugins...");

            var dllFiles = GetPluginDlls();

            foreach (var dllPath in dllFiles)
            {
                var dllP = $"{Directory.GetCurrentDirectory()}\\{dllPath}";
                var dll = LoadAssembly(dllP);
                var dllName = Path.GetFileName(dllP);

                Log($"Load plugin {dllName}");
                LoadPluginsFromAssembly(dllName, dll);
            }
        }

        public IReadOnlyCollection<TriggerTypeDataModel> Triggers => _triggerPlugins.AsReadOnly();
        public IReadOnlyCollection<TypeDataModel> Actions => _actionPlugins.AsReadOnly();

        #region Private methods

        /*private void AddInputOutput(IEnumerable<TypeDataModel> items)
        {
            foreach (var item in items)
            {
                item.Input = new Dictionary<string, string>();
                foreach (var attr in item.Type.GetMethod("DoWork")?
                             .GetCustomAttributes<InputAttribute>() ?? new List<InputAttribute>())
                {
                    if (!item.Input.ContainsKey(attr.Key))
                        item.Input.Add(attr.Key, attr.Description);
                }

                item.Output = new Dictionary<string, string>();
                foreach (var attr in item.Type.GetMethod("DoWork")?
                             .GetCustomAttributes<OutputAttribute>() ?? new List<OutputAttribute>())
                {
                    if (!item.Output.ContainsKey(attr.Key))
                        item.Output.Add(attr.Key, attr.Description);
                }
            }
        }*/

        private static IEnumerable<string> GetPluginDlls()
        {
            var dllFiles = Directory.GetFiles("Plugins", "*.dll",
                SearchOption.AllDirectories);

            return dllFiles;
        }

        private static Assembly LoadAssembly(string path)
        {
            var d = File.ReadAllBytes(path);
            return Assembly.Load(d);
        }

        private void LoadPluginsFromAssembly(string dllName, Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (!typeof(IPlugin).IsAssignableFrom(type))
                {
                    continue;
                }

                if (Activator.CreateInstance(type) is not IPlugin plugin)
                {
                    return;
                }

                plugin.Init(_services);

                AddTriggerTypes(dllName, assembly, plugin.Triggers);
                AddActionTypes(dllName, assembly, plugin.Actions);
            }
        }

        private void AddTriggerTypes(string dllName, Assembly dll, IEnumerable<TriggerTypeDataModel> triggers)
        {
            foreach (var trigger in triggers)
            {
                if (_triggerPlugins.Any(_ => _.Id == trigger.Id))
                {
                    continue;
                }

                Log($"Load trigger '{trigger.Id}' from '{dllName}'");

                _triggerPlugins.Add(trigger);

                //AddInputOutput(ev.Events.Select(e => (TypeDataModel)e));

                AddAssemblyToList(dllName, dll);
            }
        }

        private void AddActionTypes(string dllName, Assembly dll, IEnumerable<TypeDataModel> actions)
        {
            foreach (var action in actions)
            {
                if (_actionPlugins.Any(_ => _.Id == action.Id))
                {
                    continue;
                }

                Log($"Load action '{action.Id}' from '{dllName}'");

                _actionPlugins.Add(action);

                //AddInputOutput(ev.Events.Select(e => (TypeDataModel)e));

                AddAssemblyToList(dllName, dll);
            }
        }

        private void AddAssemblyToList(string dllName, Assembly dll)
        {
            if (!_assemblies.ContainsKey(dllName))
                _assemblies.Add(dllName, dll);
        }

        private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            var dllName = args.Name.Contains(',')
                ? string.Concat(args.Name.AsSpan(0, args.Name.IndexOf(',')), ".dll")
                : args.Name;

            return _assemblies.ContainsKey(dllName)
                ? _assemblies[dllName]
                : default;
        }

        private void Log(string text)
        {
            Console.WriteLine(text);
        }

        #endregion
    }
}