using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Interop;
using Wrecept.Views.InvoiceParts;
using Wrecept.ViewModels;
using Wrecept.Core.Domain;
using Wrecept.Services;
using Xunit;

namespace Wrecept.UiTests;

public class InvoiceSidebarNavigationTests
{
    private class StubDialog : IKeyboardDialogService
    {
        public bool Result { get; set; }
        public bool Confirm(string message) => Result;
        public bool ConfirmNewInvoice() => Result;
        public bool ConfirmExit() => false;
    }

    [StaFact]
    public async Task UpArrowOnFirstItem_ShouldAskForNewInvoice()
    {
        var dialog = new StubDialog { Result = true };
        App.Services = new ServiceCollection().AddSingleton<IKeyboardDialogService>(dialog).BuildServiceProvider();
        var invoices = new ObservableCollection<Invoice>
        {
            new Invoice { SerialNumber = "1" },
            new Invoice { SerialNumber = "2" }
        };
        var vm = new InvoiceSidebarViewModel(invoices, new DefaultSupplierService(new Core.Repositories.InMemorySupplierRepository()), dialog);
        vm.SelectedInvoice = invoices[0];
        await vm.NewInvoiceCommand.ExecuteAsync();
        Assert.NotNull(vm.SelectedInvoice);
        Assert.Null(vm.SelectedInvoice.Id);
    }
}
