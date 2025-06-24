using Wrecept.ViewModels;
using Wrecept.Services;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;
using Xunit;

namespace Wrecept.Tests;

public class MenuCommandInitializationTests
{
    [Fact]
    public void Commands_ShouldBeInitialized()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var nav = new NavigationService();
        var vm = new MainWindowViewModel(service, nav);

        Assert.NotNull(vm.OpenInvoiceListViewCommand);
        Assert.NotNull(vm.RefreshInvoiceDataCommand);
        Assert.NotNull(vm.OpenMasterDataViewCommand);
        Assert.NotNull(vm.FilterByDateViewCommand);
        Assert.NotNull(vm.FilterBySupplierViewCommand);
        Assert.NotNull(vm.FilterByProductGroupViewCommand);
        Assert.NotNull(vm.FilterByProductViewCommand);
        Assert.NotNull(vm.OpenHelpViewCommand);
        Assert.NotNull(vm.OpenAboutDialogCommand);
        Assert.NotNull(vm.ExitApplicationCommand);
    }
}
