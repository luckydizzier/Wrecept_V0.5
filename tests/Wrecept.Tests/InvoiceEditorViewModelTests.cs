using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using Wrecept.ViewModels;
using Wrecept.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceEditorViewModelTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var invoice = new Invoice
        {
            SerialNumber = "1",
            TransactionNumber = "T1",
            Supplier = new Supplier { Id = Guid.NewGuid(), Name = "A" },
            PaymentMethod = new PaymentMethod { Id = Guid.NewGuid(), Name = "C" }
        };

        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new InvoiceEditorViewModel(
            invoice,
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
            new NavigationService(
                service,
                new DefaultSupplierService(new InMemorySupplierRepository()),
                new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
                new DefaultProductService(new InMemoryProductRepository()),
                new DefaultProductGroupService(new InMemoryProductGroupRepository()),
                new DefaultUnitService(new InMemoryUnitRepository()),
                new DefaultTaxRateService(new InMemoryTaxRateRepository()),
                new JsonPriceHistoryService(),
                new FeedbackService(),
                new JsonSettingsService()),
            true);

        Assert.NotSame(invoice, vm.Invoice);
        Assert.Equal(invoice.SerialNumber, vm.Invoice.SerialNumber);
        Assert.True(vm.IsEditMode);
        Assert.False(vm.IsReadOnly);
    }

    [Fact]
    public void CancelEdit_ShouldRevertChanges()
    {
        var invoice = new Invoice { SerialNumber = "1", TransactionNumber = "T1" };
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new InvoiceEditorViewModel(
            invoice,
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
        vm.Invoice.SerialNumber = "2";
        vm.Invoice.Supplier = new Supplier { Id = Guid.NewGuid(), Name = "B" };
        vm.Invoice.PaymentMethod = new PaymentMethod { Id = Guid.NewGuid(), Name = "D" };

        vm.CancelEdit();

        Assert.Equal("1", vm.Invoice.SerialNumber);
        Assert.Equal("A", vm.Invoice.Supplier.Name);
        Assert.Equal("C", vm.Invoice.PaymentMethod.Name);
    }

    [Fact]
    public async Task SaveAsync_ShouldPersistAndRequestExit()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var nav = new StubNavigationService();
        var invoice = new Invoice { SerialNumber = "1", TransactionNumber = "T1" };
        var vm = new InvoiceEditorViewModel(
            invoice,
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
            nav,
            true);

        await vm.SaveAsync();

        Assert.True(vm.ExitRequested);
        Assert.Single(await repo.GetAllAsync());
        Assert.Equal(1, nav.ShowCalls);
    }

    private class StubNavigationService : INavigationService
    {
        public int ShowCalls;
        public Task ShowInvoiceListViewAsync()
        {
            ShowCalls++;
            return Task.CompletedTask;
        }

        public void SetHost(MainWindowViewModel host) { }
        public void ShowSupplierView() { }
        public void ShowProductView() { }
        public void ShowUnitView() { }
        public void ShowProductGroupView() { }
        public void ShowTaxRateView() { }
        public void ShowSettingsView() { }
        public void ShowFilterByDateView() { }
        public void ShowFilterBySupplierView() { }
        public void ShowFilterByProductGroupView() { }
        public void ShowFilterByProductView() { }
        public void ShowHelpView() { }
        public void ShowAboutDialog() { }
        public void ShowOnboardingOverlay() { }
        public void ShowSavingOverlay() { }
        public void CloseCurrentView() { }
        public void ExitApplication() { }
    }
}
