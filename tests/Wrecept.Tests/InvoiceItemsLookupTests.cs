using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceItemsLookupTests
{

    [Fact]
    public async Task OpenProductLookupAsync_ShouldSetProductName()
    {
        var vm = new InvoiceItemsViewModel(
            new Invoice(),
            new DefaultProductService(new Core.Repositories.InMemoryProductRepository()),
            new DefaultProductGroupService(new Core.Repositories.InMemoryProductGroupRepository()),
            new DefaultUnitService(new Core.Repositories.InMemoryUnitRepository()),
            new DefaultTaxRateService(new Core.Repositories.InMemoryTaxRateRepository()),
            new Infrastructure.JsonPriceHistoryService(),
            new FeedbackService());

        vm.OpenProductLookup();
        vm.ProductLookup.SelectedItem = new LookupItem<Product>(new Product { Name = "P" }, "P");
        vm.ProductLookup.Accept();

        Assert.Equal("P", vm.Entry.ProductName);
    }
}
