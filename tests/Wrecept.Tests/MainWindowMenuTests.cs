using Wrecept.ViewModels;
using Wrecept.Services;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;
using Wrecept.Views;
using System.Windows.Controls;
using Xunit;

namespace Wrecept.Tests;

public class MainWindowMenuTests
{
    [StaFact]
    public void MenuItems_ShouldBeFocusable()
    {
        var vm = new MainWindowViewModel(new DefaultInvoiceService(new InMemoryInvoiceRepository()), new NavigationService());
        var view = new MainMenu { DataContext = vm };

        foreach (MenuItem item in view.MenuBar.Items)
        {
            Assert.True(item.Focusable);
        }
    }
}
