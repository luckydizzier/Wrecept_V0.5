using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
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
    public void UpArrowOnFirstItem_ShouldAskForNewInvoice()
    {
        var dialog = new StubDialog { Result = true };
        App.Services = new ServiceCollection().AddSingleton<IKeyboardDialogService>(dialog).BuildServiceProvider();
        var invoices = new ObservableCollection<Invoice>
        {
            new Invoice { SerialNumber = "1" },
            new Invoice { SerialNumber = "2" }
        };
        var vm = new InvoiceSidebarViewModel(invoices, new DefaultSupplierService(new Core.Repositories.InMemorySupplierRepository()));
        var sidebar = new InvoiceSidebar { DataContext = vm };
        sidebar.Measure(new System.Windows.Size(100,100));
        sidebar.Arrange(new System.Windows.Rect(0,0,100,100));
        var method = typeof(InvoiceSidebar).GetMethod("InvoiceList_OnPreviewKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var source = new HwndSource(new HwndSourceParameters());
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, Key.Up) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
        method.Invoke(sidebar, new object[] { sidebar, args });
        Assert.NotNull(vm.SelectedInvoice);
        Assert.Null(vm.SelectedInvoice.Id);
    }
}
