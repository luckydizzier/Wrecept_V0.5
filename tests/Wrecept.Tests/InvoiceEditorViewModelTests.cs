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

        Assert.Equal(invoice, vm.Invoice);
        Assert.True(vm.IsEditMode);
        Assert.False(vm.IsReadOnly);
    }
}
