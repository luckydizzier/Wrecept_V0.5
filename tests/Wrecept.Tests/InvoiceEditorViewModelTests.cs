using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceEditorViewModelTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var invoice = new Invoice { SerialNumber = "1" };

        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new InvoiceEditorViewModel(invoice, true, service);

        Assert.NotSame(invoice, vm.Invoice);
        Assert.Equal(invoice.SerialNumber, vm.Invoice.SerialNumber);
        Assert.True(vm.IsEditMode);
        Assert.False(vm.IsReadOnly);
    }

    [Fact]
    public void CancelEdit_ShouldRevertChanges()
    {
        var invoice = new Invoice { SerialNumber = "1" };
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new InvoiceEditorViewModel(invoice, true, service);
        vm.Invoice.SerialNumber = "2";

        vm.CancelEdit();

        Assert.Equal("1", vm.Invoice.SerialNumber);
    }

    [Fact]
    public async Task SaveAsync_ShouldPersistAndRequestExit()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var invoice = new Invoice { SerialNumber = "1" };
        var vm = new InvoiceEditorViewModel(invoice, true, service);

        await vm.SaveAsync();

        Assert.True(vm.ExitRequested);
        Assert.Single(await repo.GetAllAsync());
    }
}
