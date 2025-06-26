using System;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceSidebarViewModelTests
{
    [Fact]
    public void Filter_ShouldReturnMatchingInvoices()
    {
        var supplierService = new DefaultSupplierService(new Core.Repositories.InMemorySupplierRepository());
        var invoices = new ObservableCollection<Invoice>
        {
            new Invoice { SerialNumber = "A1", IssueDate = new DateOnly(2024,1,1), Supplier = new Supplier { Id = Guid.NewGuid() } },
            new Invoice { SerialNumber = "B2", IssueDate = new DateOnly(2024,2,1), Supplier = new Supplier { Id = Guid.NewGuid() } }
        };
        var vm = new InvoiceSidebarViewModel(invoices, supplierService)
        {
            SearchText = "B2"
        };

        Assert.Single(vm.Invoices);
        Assert.Equal("B2", vm.Invoices[0].SerialNumber);
    }
}
