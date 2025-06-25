using System.Linq;
using Wrecept.Core.Plugins;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class PluginMenuItemTests
{
    private class TestPlugin : IMenuPlugin
    {
        public bool Executed { get; private set; }
        public string MenuHeader => "Teszt";
        public void Execute() => Executed = true;
    }

    [Fact]
    public void PluginMenuItemViewModel_ShouldInvokeExecute()
    {
        var plugin = new TestPlugin();
        var vm = new PluginMenuItemViewModel(plugin);

        vm.Command.Execute(null);

        Assert.True(plugin.Executed);
    }
}
