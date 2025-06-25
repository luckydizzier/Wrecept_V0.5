using System.Collections.Generic;
using Wrecept.Core.Plugins;
using Wrecept.ViewModels;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;
using Wrecept.Services;
using Xunit;

namespace Wrecept.Tests;

public class MainWindowPluginTests
{
    private class DummyPlugin : IMenuPlugin
    {
        public string MenuHeader => "Dummy";
        public void Execute() {}
    }

    [Fact]
    public void Constructor_ShouldCreatePluginMenuItems()
    {
        var plugin = new DummyPlugin();
        var vm = new MainWindowViewModel(new DefaultInvoiceService(new InMemoryInvoiceRepository()), new NavigationService(), new[] { plugin });

        Assert.Single(vm.PluginMenuItems);
        Assert.Equal("Dummy", vm.PluginMenuItems[0].Header);
    }
}
