using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceItemsLookupTests
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
    public async Task OpenProductLookupAsync_ShouldSetProductName()
    {
        var presenter = new StubPresenter
        {
            Result = true,
            Selected = new LookupItem<Product>(new Product { Name = "P" }, "P")
        };
        var vm = new InvoiceItemsViewModel(new Invoice(), new DefaultProductService(new Core.Repositories.InMemoryProductRepository()), new DefaultProductGroupService(new Core.Repositories.InMemoryProductGroupRepository()), new DefaultUnitService(new Core.Repositories.InMemoryUnitRepository()), new DefaultTaxRateService(new Core.Repositories.InMemoryTaxRateRepository()), presenter);

        await vm.OpenProductLookupAsync();

        Assert.Equal("P", vm.Entry.ProductName);
    }
}
