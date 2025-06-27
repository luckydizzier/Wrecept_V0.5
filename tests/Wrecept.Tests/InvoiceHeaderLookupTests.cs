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

    [Fact]
    public async Task OpenSupplierLookupAsync_ShouldAssignSelectedSupplier()
    {
        var service = new DefaultSupplierService(new Core.Repositories.InMemorySupplierRepository());
        var pmRepo = new Core.Repositories.InMemoryPaymentMethodRepository();
        var pmService = new DefaultPaymentMethodService(pmRepo);
        await pmService.SaveAsync(new PaymentMethod { Label = "Cash" });
        var invoice = new Invoice { Supplier = new Supplier() };
        var vm = new InvoiceHeaderViewModel(invoice, pmService, service);

        vm.OpenSupplierLookup();
        vm.SupplierLookup.SelectedItem = new LookupItem<Supplier>(new Supplier { Name = "Teszt" }, "Teszt");
        vm.SupplierLookup.Accept();

        Assert.Equal("Teszt", invoice.Supplier.Name);
    }
}
