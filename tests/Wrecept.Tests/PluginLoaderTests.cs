using System.IO;
using System.Linq;
using Wrecept.Core.Plugins;
using Xunit;

namespace Wrecept.Tests;

public class PluginLoaderTests
{
    [Fact]
    public void LoadPlugins_ShouldFindGreetingPlugin()
    {
        var pluginDir = Path.Combine(Directory.GetCurrentDirectory(), "../../../..", "src", "Wrecept.Plugin.Greeting", "bin", "Debug", "net8.0-windows");
        var plugins = PluginLoader.LoadPlugins(pluginDir).ToList();

        Assert.Contains(plugins, p => p.MenuHeader == "Köszöntés");
    }
}
