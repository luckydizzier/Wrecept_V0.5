using Wrecept.Core.Domain;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceEditorViewModelTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var invoice = new Invoice { SerialNumber = "1" };

        var vm = new InvoiceEditorViewModel(invoice, true);

        Assert.NotSame(invoice, vm.Invoice);
        Assert.Equal(invoice.SerialNumber, vm.Invoice.SerialNumber);
        Assert.True(vm.IsEditMode);
        Assert.False(vm.IsReadOnly);
    }

    [Fact]
    public void CancelEdit_ShouldRevertChanges()
    {
        var invoice = new Invoice { SerialNumber = "1" };
        var vm = new InvoiceEditorViewModel(invoice, true);
        vm.Invoice.SerialNumber = "2";

        vm.CancelEdit();

        Assert.Equal("1", vm.Invoice.SerialNumber);
    }
}
