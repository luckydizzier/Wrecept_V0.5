using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceItemsCommandTests
{
    [Fact]
    public void StartEditCommand_ShouldOpenProductLookup()
    {
        var vm = new InvoiceItemsViewModel(
            new Invoice(),
            new DefaultProductService(new Core.Repositories.InMemoryProductRepository()),
            new DefaultProductGroupService(new Core.Repositories.InMemoryProductGroupRepository()),
            new DefaultUnitService(new Core.Repositories.InMemoryUnitRepository()),
            new DefaultTaxRateService(new Core.Repositories.InMemoryTaxRateRepository()),
            new Infrastructure.JsonPriceHistoryService(),
            new FeedbackService());

        vm.StartEditCommand.Execute(0);

        Assert.True(vm.ProductLookup.IsDropDownOpen);
    }

    [Fact]
    public async Task ConfirmEntryCommand_ShouldAddItem_WhenOnVatColumn()
    {
        var invoice = new Invoice();
        var vm = new InvoiceItemsViewModel(
            invoice,
            new DefaultProductService(new Core.Repositories.InMemoryProductRepository()),
            new DefaultProductGroupService(new Core.Repositories.InMemoryProductGroupRepository()),
            new DefaultUnitService(new Core.Repositories.InMemoryUnitRepository()),
            new DefaultTaxRateService(new Core.Repositories.InMemoryTaxRateRepository()),
            new Infrastructure.JsonPriceHistoryService(),
            new FeedbackService());

        vm.Entry.ProductName = "P";
        vm.Entry.Quantity = 1;
        vm.Entry.UnitName = "db";
        vm.Entry.UnitPriceNet = 100m;
        vm.Entry.VatRatePercent = 27m;

        await vm.ConfirmEntryCommand.ExecuteAsync(4);

        Assert.Single(invoice.Items);
    }

    [Fact]
    public void CancelEntryCommand_ShouldClearEntry()
    {
        var vm = new InvoiceItemsViewModel(
            new Invoice(),
            new DefaultProductService(new Core.Repositories.InMemoryProductRepository()),
            new DefaultProductGroupService(new Core.Repositories.InMemoryProductGroupRepository()),
            new DefaultUnitService(new Core.Repositories.InMemoryUnitRepository()),
            new DefaultTaxRateService(new Core.Repositories.InMemoryTaxRateRepository()),
            new Infrastructure.JsonPriceHistoryService(),
            new FeedbackService());

        vm.Entry.ProductName = "X";

        vm.CancelEntryCommand.Execute(null);

        Assert.Equal(string.Empty, vm.Entry.ProductName);
    }
}
