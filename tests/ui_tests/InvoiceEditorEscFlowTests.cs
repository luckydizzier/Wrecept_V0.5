using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.UiTests;

public class InvoiceEditorEscFlowTests
{
    [Fact]
    public void CancelByEsc_ShouldSetFlags()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new InvoiceEditorViewModel(new Invoice(), true, service);

        vm.CancelByEsc();

        Assert.True(vm.ExitRequested);
        Assert.True(vm.ExitedByEsc);
    }

    [Fact]
    public async Task SaveAsync_ShouldNotSetExitedByEsc()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var vm = new InvoiceEditorViewModel(new Invoice(), true, service);

        await vm.SaveAsync();

        Assert.False(vm.ExitedByEsc);
    }
}
