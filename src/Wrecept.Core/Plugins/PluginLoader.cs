using System.Reflection;

namespace Wrecept.Core.Plugins;

public static class PluginLoader
{
    public static IEnumerable<IMenuPlugin> LoadPlugins(string? pluginDirectory = null)
    {
        pluginDirectory ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        if (!Directory.Exists(pluginDirectory))
            return Enumerable.Empty<IMenuPlugin>();

        var plugins = new List<IMenuPlugin>();
        foreach (var file in Directory.GetFiles(pluginDirectory, "Wrecept.Plugin.*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(t => typeof(IMenuPlugin).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                {
                    if (Activator.CreateInstance(type) is IMenuPlugin plugin)
                        plugins.Add(plugin);
                }
            }
            catch
            {
                // invalid plugin ignored
            }
        }
        return plugins;
    }
}
