using System.Windows;
using System.Media;
using System.Windows.Input;
using Wrecept.Core.Domain;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;
using Wrecept.Services;
using Wrecept.ViewModels;

namespace Wrecept;

public partial class MainWindow : Window
{
    private bool _confirmDialogOpen;

    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is ViewModels.MainWindowViewModel vm)
        {
            _ = vm.LoadInvoicesAsync();
        }
    }

    private void InvoiceGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;

        vm.EnsureValidSelection();

        if (e.Key == Key.Enter && vm.SelectedInvoice is not null)
        {
            var editorVm = new InvoiceEditorViewModel(vm.SelectedInvoice, false);
            var view = new InvoiceEditorView { DataContext = editorVm, Owner = this };
            view.ShowDialog();
            e.Handled = true;
        }
        else if (e.Key == Key.Up && InvoiceGrid.SelectedIndex == 0 && !_confirmDialogOpen)
        {
            _confirmDialogOpen = true;
            var dialogService = WreceptAppContext.DialogService;
            var create = dialogService.ConfirmNewInvoice();
            e.Handled = true;
            _confirmDialogOpen = false;
            if (create)
            {
                var editorVm = new InvoiceEditorViewModel(new Invoice(), true);
                var view = new InvoiceEditorView { DataContext = editorVm, Owner = this };
                view.ShowDialog();
            }
            else
            {
                SystemSounds.Beep.Play();
                vm.StatusMessage = "Lista teteje";
            }
        }
        else if (e.Key == Key.Down)
        {
            if (!vm.MoveSelectionDown())
            {
                SystemSounds.Beep.Play();
                vm.StatusMessage = "Lista vége";
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Up)
        {
            if (!vm.MoveSelectionUp())
            {
                SystemSounds.Beep.Play();
                vm.StatusMessage = "Lista teteje";
            }
            e.Handled = true;
        }
    }
}
