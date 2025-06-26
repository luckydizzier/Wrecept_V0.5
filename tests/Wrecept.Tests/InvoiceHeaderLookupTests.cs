using System.Collections.Generic;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceHeaderLookupTests
{
    private class StubPresenter : ILookupDialogPresenter
    {
        public bool? Result { get; set; }
        public object? Selected { get; set; }
        public bool? ShowDialog<T>(LookupDialogViewModel<T> vm)
        {
            vm.SelectedItem = Selected as LookupItem<T>;
            return Result;
        }
    }

    [Fact]
    public async Task OpenSupplierLookupAsync_ShouldAssignSelectedSupplier()
    {
        var service = new DefaultSupplierService(new Core.Repositories.InMemorySupplierRepository());
        var presenter = new StubPresenter
        {
            Result = true,
            Selected = new LookupItem<Supplier>(new Supplier { Name = "Teszt" }, "Teszt")
        };
        var pmRepo = new Core.Repositories.InMemoryPaymentMethodRepository();
        var pmService = new DefaultPaymentMethodService(pmRepo);
        await pmService.SaveAsync(new PaymentMethod { Label = "Cash" });
        var invoice = new Invoice { Supplier = new Supplier() };
        var vm = new InvoiceHeaderViewModel(invoice, pmService, service, presenter);

        await vm.OpenSupplierLookupAsync();

        Assert.Equal("Teszt", invoice.Supplier.Name);
    }
}
