using System.Reflection;
using System.Windows.Input;
using System.Windows.Interop;
using Wrecept.ViewModels;
using Wrecept.Services;
using Wrecept.Core.Services;
using Wrecept.Core.Repositories;
using Wrecept;
using Xunit;

namespace Wrecept.Tests;

public class ExitConfirmationTests
{
    private class StubDialog : IKeyboardDialogService
    {
        public bool ExitAsked { get; private set; }
        public bool Confirm(string message) => false;
        public bool ConfirmNewInvoice() => false;
        public bool ConfirmExit() { ExitAsked = true; return false; }
    }

    [StaFact]
    public void EscOnMenu_ShouldAskForExit()
    {
        var dialog = new StubDialog();
        Infrastructure.AppContext.DialogService = dialog;
        Infrastructure.AppContext.NavigationService = new NavigationService();
        Infrastructure.AppContext.InputLocked = false;

        var window = new MainWindow { DataContext = new MainWindowViewModel(new DefaultInvoiceService(new InMemoryInvoiceRepository()), new NavigationService()) };
        var method = typeof(MainWindow).GetMethod("MainWindow_OnPreviewKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var source = new HwndSource(new HwndSourceParameters());
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, Key.Escape) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
        window.MenuBar.Focus();
        method.Invoke(window, new object[] { window, args });

        Assert.True(dialog.ExitAsked);
        window.Close();
    }
}
