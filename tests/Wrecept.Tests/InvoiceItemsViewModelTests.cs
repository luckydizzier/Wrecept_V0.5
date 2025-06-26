using Wrecept.Core.Domain;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceItemsViewModelTests
{
    [Fact]
    public void AddItemCommand_ShouldAddRowAndClearEntry()
    {
        var invoice = new Invoice();
        var vm = new InvoiceItemsViewModel(invoice);

        vm.Entry.ProductName = "Teszt";
        vm.Entry.Quantity = 2;
        vm.Entry.UnitName = "db";
        vm.Entry.UnitPriceNet = 100m;
        vm.Entry.VatRatePercent = 27m;

        vm.AddItemCommand.Execute(null);

        Assert.Single(invoice.Items);
        Assert.Equal(string.Empty, vm.Entry.ProductName);
        Assert.Equal(2, vm.Rows.Count);
    }

    [Fact]
    public void AddItemCommand_ShouldIgnoreInvalidEntry()
    {
        var invoice = new Invoice();
        var vm = new InvoiceItemsViewModel(invoice);

        vm.AddItemCommand.Execute(null);

        Assert.Empty(invoice.Items);
        Assert.Single(vm.Rows);
    }
}
