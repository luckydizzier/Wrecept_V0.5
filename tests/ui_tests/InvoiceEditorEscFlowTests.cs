using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Wrecept.Services;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.UiTests;

public class InvoiceEditorEscFlowTests
{
    private class StubDialog : IKeyboardDialogService
    {
        public bool Result { get; set; }
        public bool Confirm(string message) => Result;
        public bool ConfirmNewInvoice() => false;
        public bool ConfirmExit() => false;
    }
    [Fact]
    public async Task CancelByEsc_ShouldSetFlags()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var dialog = new StubDialog { Result = true };
        var vm = new InvoiceEditorViewModel(
            new Invoice
            {
                SerialNumber = "1",
                TransactionNumber = "T1",
                Supplier = new Supplier { Name = "A" },
                PaymentMethod = new PaymentMethod { Label = "Cash" },
                Items = { new InvoiceItem { Id = Guid.NewGuid(), Product = new Product { Name = "P" }, Quantity = 1, Unit = new Unit { Name = "db" }, UnitPriceNet = 100, VatRatePercent = 27 } }
            },
            true,
            service,
            new DefaultSupplierService(new InMemorySupplierRepository()),
            new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
            new DefaultProductService(new InMemoryProductRepository()),
            new DefaultProductGroupService(new InMemoryProductGroupRepository()),
            new DefaultUnitService(new InMemoryUnitRepository()),
            new DefaultTaxRateService(new InMemoryTaxRateRepository()),
            new JsonPriceHistoryService(),
            new FeedbackService(),
            dialog,
            new NavigationService(),
            true);

        await vm.CancelByEscAsync();

        Assert.True(vm.ExitRequested);
    }

    [Fact]
    public async Task SaveAsync_ShouldNotSetExitedByEsc()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var vm = new InvoiceEditorViewModel(
            new Invoice(),
            true,
            service,
            new DefaultSupplierService(new InMemorySupplierRepository()),
            new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
            new DefaultProductService(new InMemoryProductRepository()),
            new DefaultProductGroupService(new InMemoryProductGroupRepository()),
            new DefaultUnitService(new InMemoryUnitRepository()),
            new DefaultTaxRateService(new InMemoryTaxRateRepository()),
            new JsonPriceHistoryService(),
            new FeedbackService(),
            new KeyboardDialogService(),
            new NavigationService(),
            true);

        await vm.SaveAsync();

        Assert.False(vm.ExitedByEsc);
    }
}
