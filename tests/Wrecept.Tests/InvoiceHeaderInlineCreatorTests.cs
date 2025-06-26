using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.ViewModels;
using Xunit;
using System.Threading.Tasks;

namespace Wrecept.Tests;

public class InvoiceHeaderInlineCreatorTests
{
    private class StubPresenter : ILookupDialogPresenter
    {
        public bool? ShowDialog<T>(LookupDialogViewModel<T> vm) => null;
    }
    [Fact]
    public async Task TryOpenSupplierCreator_ShouldReturnTrue_WhenNameMissing()
    {
        var repo = new Wrecept.Core.Repositories.InMemorySupplierRepository();
        var service = new DefaultSupplierService(repo);
        var pmRepo = new Wrecept.Core.Repositories.InMemoryPaymentMethodRepository();
        var pmService = new DefaultPaymentMethodService(pmRepo);
        await pmService.SaveAsync(new PaymentMethod { Label = "Készpénz" });
        var invoice = new Invoice { Supplier = new Supplier { Name = "Új Szállító" } };
        var vm = new InvoiceHeaderViewModel(invoice, pmService, service, new StubPresenter());

        var opened = await vm.TryOpenSupplierCreatorAsync();

        Assert.True(opened);
        Assert.NotNull(vm.SupplierCreator);
    }
}
