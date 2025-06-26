using System.Windows;
using System.Media;
using System.Windows.Input;
using Wrecept.Core.Domain;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;
using Wrecept.Services;
using Wrecept.ViewModels;
using Wrecept.Views;

namespace Wrecept;

public partial class MainWindow : Window
{
    private bool _confirmDialogOpen;
    private DateTime _lastEnter = DateTime.MinValue;
    private bool _escOnce;

    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is ViewModels.MainWindowViewModel vm)
        {
            WreceptAppContext.StatusMessageSetter = msg => vm.StatusMessage = msg;
            _ = vm.LoadInvoicesAsync();
        }
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;

        if (MenuBar.IsKeyboardFocusWithin && e.Key == Key.Escape)
        {
            Infrastructure.AppContext.InputLocked = true;
            var confirm = WreceptAppContext.DialogService.ConfirmExit();
            Infrastructure.AppContext.InputLocked = false;
            e.Handled = true;
            if (confirm)
            {
                WreceptAppContext.NavigationService.ExitApplication();
            }
        }
    }

    private void InvoiceGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;
        if (DataContext is not MainWindowViewModel vm) return;

        if (vm.Invoices.Count == 0)
        {
            Infrastructure.AppContext.FeedbackService.Error();
            vm.StatusMessage = "Nincs tétel - F2: új hozzáadás";
            e.Handled = true;
            return;
        }

        vm.EnsureValidSelection();

        if (e.Key == Key.Enter && vm.SelectedInvoice is not null)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastEnter).TotalMilliseconds < 300)
            {
                e.Handled = true;
                return;
            }
            _lastEnter = now;
            Infrastructure.AppContext.InputLocked = true;
            var editorVm = new InvoiceEditorViewModel(vm.SelectedInvoice, false, WreceptAppContext.InvoiceService);
            var view = new InvoiceEditorWindow { DataContext = editorVm, Owner = this };
            view.Loaded += (_, _) => editorVm.OnLoaded();
            var id = vm.SelectedInvoice.Id;
            view.ShowDialog();
            vm.RestoreSelection(id);
            _escOnce = editorVm.ExitedByEsc;
            Infrastructure.AppContext.InputLocked = false;
            e.Handled = true;
        }
        else if (e.Key == Key.Up && InvoiceGrid.SelectedIndex == 0 && !_confirmDialogOpen)
        {
            Infrastructure.AppContext.InputLocked = true;
            _confirmDialogOpen = true;
            var dialogService = WreceptAppContext.DialogService;
            var create = dialogService.ConfirmNewInvoice();
            e.Handled = true;
            _confirmDialogOpen = false;
            Infrastructure.AppContext.InputLocked = false;
            if (create)
            {
                Infrastructure.AppContext.FeedbackService.Accept();
                Infrastructure.AppContext.InputLocked = true;
                var editorVm = new InvoiceEditorViewModel(new Invoice(), true, WreceptAppContext.InvoiceService);
                var view = new InvoiceEditorWindow { DataContext = editorVm, Owner = this };
                view.Loaded += (_, _) => editorVm.OnLoaded();
                view.ShowDialog();
                _escOnce = editorVm.ExitedByEsc;
                Infrastructure.AppContext.InputLocked = false;
            }
            else
            {
                Infrastructure.AppContext.FeedbackService.Reject();
                vm.StatusMessage = "Lista teteje";
            }
        }
        else if (e.Key == Key.Down)
        {
            if (!vm.MoveSelectionDown())
            {
                Infrastructure.AppContext.FeedbackService.Error();
                vm.StatusMessage = "Lista vége";
            }
            e.Handled = true;
        }
        else if (e.Key == Key.PageDown)
        {
            if (!vm.MovePageDown())
            {
                Infrastructure.AppContext.FeedbackService.Error();
                vm.StatusMessage = "Lista vége";
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Up)
        {
            if (!vm.MoveSelectionUp())
            {
                Infrastructure.AppContext.FeedbackService.Error();
                vm.StatusMessage = "Lista teteje";
            }
            e.Handled = true;
        }
        else if (e.Key == Key.PageUp)
        {
            if (!vm.MovePageUp())
            {
                Infrastructure.AppContext.FeedbackService.Error();
                vm.StatusMessage = "Lista teteje";
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            if (vm.SelectedInvoice is null || _escOnce)
            {
                MenuBar.Focus();
                vm.StatusMessage = "Fókusz: főmenü";
                _escOnce = false;
            }
            else
            {
                vm.SelectedInvoice = null;
                _escOnce = true;
            }
            e.Handled = true;
        }
    }
}
