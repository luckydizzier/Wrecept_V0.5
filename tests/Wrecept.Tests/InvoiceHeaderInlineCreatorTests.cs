using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceHeaderInlineCreatorTests
{
    [Fact]
    public void TryOpenSupplierCreator_ShouldReturnTrue_WhenNameMissing()
    {
        var repo = new Wrecept.Core.Repositories.InMemorySupplierRepository();
        var service = new DefaultSupplierService(repo);
        var invoice = new Invoice { Supplier = new Supplier { Name = "Új Szállító" } };
        var vm = new InvoiceHeaderViewModel(invoice, new[] { "Készpénz" }, new[] { "Nettó" }, service);

        var opened = vm.TryOpenSupplierCreator();

        Assert.True(opened);
        Assert.NotNull(vm.SupplierCreator);
    }
}
