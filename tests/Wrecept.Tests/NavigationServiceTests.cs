using Wrecept.Services;
using Wrecept.Infrastructure;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;
using System.Windows;
using Xunit;

namespace Wrecept.Tests;

public class NavigationServiceTests
{
    private class StubNavigationService : NavigationService
    {
        public int Calls;
        protected override void ShowDialog(Window view)
        {
            Calls++;
        }
    }

    [StaFact]
    public void ShowSupplierView_ShouldToggleInputLock()
    {
        AppContext.SupplierService = new DefaultSupplierService(new InMemorySupplierRepository());
        var nav = new StubNavigationService();
        AppContext.InputLocked = false;

        nav.ShowSupplierView();

        Assert.False(AppContext.InputLocked);
        Assert.Equal(1, nav.Calls);
    }
}
