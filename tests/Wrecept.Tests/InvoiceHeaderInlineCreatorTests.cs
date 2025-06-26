using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;
using System.Threading.Tasks;

namespace Wrecept.Tests;

public class InvoiceHeaderInlineCreatorTests
{
    [Fact]
    public async Task TryOpenSupplierCreator_ShouldReturnTrue_WhenNameMissing()
    {
        var repo = new Wrecept.Core.Repositories.InMemorySupplierRepository();
        var service = new DefaultSupplierService(repo);
        var invoice = new Invoice { Supplier = new Supplier { Name = "Új Szállító" } };
        var vm = new InvoiceHeaderViewModel(invoice, new[] { "Készpénz" }, new[] { "Nettó" }, service);

        var opened = await vm.TryOpenSupplierCreatorAsync();

        Assert.True(opened);
        Assert.NotNull(vm.SupplierCreator);
    }
}
