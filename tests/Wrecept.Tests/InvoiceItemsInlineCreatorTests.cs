using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceItemsInlineCreatorTests
{
    [Fact]
    public void TryOpenProductCreator_ShouldReturnTrue_WhenNameMissing()
    {
        var invoice = new Invoice();
        var repo = new Wrecept.Core.Repositories.InMemoryProductRepository();
        var vm = new InvoiceItemsViewModel(invoice,
            new DefaultProductService(repo),
            new DefaultProductGroupService(new Wrecept.Core.Repositories.InMemoryProductGroupRepository()),
            new DefaultUnitService(new Wrecept.Core.Repositories.InMemoryUnitRepository()),
            new DefaultTaxRateService(new Wrecept.Core.Repositories.InMemoryTaxRateRepository()));

        vm.Entry.ProductName = "NewProd";

        var opened = vm.TryOpenProductCreator();

        Assert.True(opened);
        Assert.NotNull(vm.ProductCreator);
    }
}
